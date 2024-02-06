using Microsoft.AspNetCore.Mvc;
using MongoDB.Utilities;
using MongoDBTest.Blogic.Authentication;

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
            var allUsers = await _userService.GetAsync();
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

}