using Microsoft.AspNetCore.Mvc;
using MongoDBTest.Models;
using MongoDBTest.Services;


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
        public async Task<IActionResult> Get()
        {
            try
            {
                var allProducts = await _productService.GetAsync();
                return Ok(allProducts);
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

                if (existingProduct == null)
                    return NotFound();

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
                var existingProduct = await _productService.GetIdAsync(id);

                if (existingProduct == null)
                    return NotFound();

                product.Id = existingProduct.Id;

                await _productService.UpDateAsync(product);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
