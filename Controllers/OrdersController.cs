using Microsoft.AspNetCore.Mvc;
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
            // If pageNumber or itemsPerPage are not provided, set default values
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            itemsPerPage = itemsPerPage == 0 ? 10 : itemsPerPage;

            // Get all products from the service
            var allOrders = await  _orderService.GetAsync();
            int totalItems = allOrders.Count;

            // If no products are found, return No Content status
            if (totalItems <= 0)
                return NoContent(); // 204 No Content

            // Calculate the total number of pages
            int totaltPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            // Create a PagedResult object with the pagination data and the products for the current page
            var result = new PagedResult<Order>
            {
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotaltPages = totaltPages,
                // Skip the products of the previous pages and take the products for the current page
                Items = allOrders.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
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

