using Microsoft.AspNetCore.Mvc;
using MongoDB.Utilities;
using MongoDBTest.Blogic.Services;
using MongoDBTest.Models;

namespace MongoDB.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    // OrderService instance
    private readonly OrderService _orderService;

    // Constructor for the OrdersController
    public OrdersController(OrderService orderService)
    {
        // If orderService is null, throw an exception
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    // HTTP GET method to get a list of all orders
    [HttpGet]
   public async Task<IActionResult> Get(int pageNumber,int itemsPerPage)
    {
        try
        {
            var allUsers = await _orderService.GetAsync();
            var result = PaginationHelper.Paginate(allUsers, pageNumber, itemsPerPage);

            if (result == null)
                return NoContent();
            // Return the result with OK status
            return Ok(result);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // HTTP GET method to get an order by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            // Get the order from the service
            var order = await _orderService.GetIdAsync(id);

            // If the order is not found, return Not Found status
            if (order == null)
                return NotFound(); // 404 Not Found

            // If the order is found, return it with OK status
            return Ok(order);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

