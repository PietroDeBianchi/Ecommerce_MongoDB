using Microsoft.AspNetCore.Mvc;
using MongoDB.Utilities;

namespace MongoDBTest.Blogic.Authentication;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;
    private readonly GenerateJwt _jwtGenerator;

    public UserController(UserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
        _jwtGenerator = new GenerateJwt(configuration); // Initialize GenerateJwt instance
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        try
        {
            if (user.Email == null || user.Password == null)
                return BadRequest("Email and password are required");
                
            User registerUser = await _userService.RegisterAsync(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        try
        {
            // Authenticate the user
            var logUser = await _userService.LogInAsync(email, password);
            if (logUser == null)
            {
                return Unauthorized("Invalid email or password");
            }
            // Generate JWT token
            var token = _jwtGenerator.GenerateJwtToken(logUser);
            // Return the token to the client
            return Ok(new { Token = token });       
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}