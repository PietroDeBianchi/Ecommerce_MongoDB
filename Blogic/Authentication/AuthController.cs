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
            // Get all users from the service and paginate them
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

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        var registeredUser = await _userService.Register(user);
        return Ok(registeredUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var token = await _userService.Login(email, password);
        if (token == null)
        {
            return Unauthorized();
        }
        return Ok(new { Token = token });
    }

    [HttpPost("logout")]
    public Task<IActionResult> Logout(string token)
    {
        var result = _userService.Logout(token);
        throw new NotImplementedException();
    }

}