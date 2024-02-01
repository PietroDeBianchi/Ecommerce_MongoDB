using Microsoft.AspNetCore.Mvc;
using MongoDBTest.Blogic.Services;

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
    public async Task<IActionResult> Get()
    {
        try
        {
            // Get all orders from the service
            var allOrders = await _orderService.GetAsync();

            // If no orders are found, return No Content status
            if (allOrders == null)
                return NoContent(); // 204 No Content

            // If orders are found, return them with OK status
            return Ok(allOrders);
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

