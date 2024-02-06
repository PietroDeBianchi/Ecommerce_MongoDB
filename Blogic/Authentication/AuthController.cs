using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDBTest.Blogic.Authentication;
using MongoDBTest.Models;

namespace MongoDB.Blogic.Authentication;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int pageNumber,int itemsPerPage)
    {
        try
        {
            // If pageNumber or itemsPerPage are not provided, set default values
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            itemsPerPage = itemsPerPage == 0 ? 10 : itemsPerPage;

            // Get all users from the service
            var allUsers = await _userService.GetAsync();
            int totalItems = allUsers.Count;

            // If no users are found, return No Content status
            if (totalItems <= 0)
                return NoContent(); // 204 No Content

            // Calculate the total number of pages
            int totaltPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            // Create a PagedResult object with the pagination data and the products for the current page
            var result = new PagedResult<User>
            {
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems,
                TotaltPages = totaltPages,
                // Skip the users of the previous pages and take the products for the current page
                Items = allUsers.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage)
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

}