using Microsoft.AspNetCore.Mvc;
using MongoDBTest.Models;
using MongoDBTest.Blogic.Services;

namespace MongoDB.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    // ProductService instance
    private readonly ProductService _productService;

    // Constructor for the ProductsController
    public ProductsController(ProductService productService)
    {
        // If productService is null, throw an exception
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
    }

    // HTTP GET method to get a list of products with pagination
    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber,int itemsPerPage)
    {
        try
        {
            // If pageNumber or itemsPerPage are not provided, set default values
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            itemsPerPage = itemsPerPage == 0 ? 10 : itemsPerPage;

            // Get all products from the service
            var allProducts = await _productService.GetAsync();
            int totalItems = allProducts.Count;

            // If no products are found, return No Content status
            if (totalItems <= 0)
                return NoContent(); // 204 No Content

            // Calculate the total number of pages
            int totaltPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            // Create a PagedResult object with the pagination data and the products for the current page
            var result = new PagedResult<Product>
            {
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotaltPages = totaltPages,
                // Skip the products of the previous pages and take the products for the current page
                Items = allProducts.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
            };

            // Return the result with OK status
            return Ok(result);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // HTTP GET method to get a product by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            // Get the product from the service
            var product = await _productService.GetIdAsync(id);

            // If the product is not found, return Not Found status
            if (product == null)
                return NotFound(); // 404 Not Found

            // If the product is found, return it with OK status
            return Ok(product);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // HTTP DELETE method to delete a product by its ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            // Get the existing product from the service
            var existingProduct = await _productService.GetIdAsync(id);

            // Delete the product
            await _productService.DeleteAsync(id);

            // Return No Content status
            return NoContent();
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // HTTP POST method to create a new product
    [HttpPost]
    public async Task<IActionResult> Post(Product product)
    {
        try
        {
            // Create the product
            await _productService.CreateAsync(product);

            // Return Created At Action status with the location of the new product
            return CreatedAtAction(nameof(GetById), new { id = product.productCode }, product);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // HTTP PUT method to update an existing product
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Product product)
    {
        try
        {
            // Update the product
            await _productService.UpDateAsync(id, product);

            // Return No Content status
            return NoContent();
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

