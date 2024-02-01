using Microsoft.AspNetCore.Mvc;
using MongoDBTest.Models;
using MongoDBTest.Blogic.Services;

// $lookup {
//   from: "orderdetail",
//   localField: "productCode",
//   foreignField: "productCode",
//   as: "orderDetails",
// }

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber,int itemsPerPage)
        {
            try
            {
                // Define pagination variables if they are not provided
                pageNumber = pageNumber == 0 ? 1 : pageNumber;
                itemsPerPage = itemsPerPage == 0 ? 10 : itemsPerPage;
                // Define pagination data
                var allProducts = await _productService.GetAsync();
                int totalItems = allProducts.Count;
                int totaltPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
                // set Pagination headers
                var result = new PagedResult<Product>
                {
                    PageNumber = pageNumber,
                    ItemsPerPage = itemsPerPage,
                    TotalItems = totalItems,
                    TotaltPages = totaltPages,
                    Items = allProducts.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var product = await _productService.GetIdAsync(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var existingProduct = await _productService.GetIdAsync(id);
                await _productService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            try
            {
                await _productService.CreateAsync(product);
                return CreatedAtAction(nameof(GetById), new { id = product.productCode }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, Product product)
        {
            try
            {
                await _productService.UpDateAsync(id, product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
