using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

    [HttpPost("register")]
    public ActionResult<User> Register(User user)
    {
        _userService.Register(user);
        return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
    }

    [HttpPost("login")]
    public IActionResult Login(User userParam)
    {
        User user = userParam;

        if (userParam != null && userParam.Email != null && userParam.Password != null)
        {
            user = _userService.Authenticate(userParam.Email, userParam.Password);
        }
        else
        {
            // Handle the case where userParams or its properties are null
            return BadRequest("Invalid user parameters");
        }
        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("YOUR_SECRET_KEY");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] 
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new {
            Id = user.Id,
            Email = user.Email,
            Token = tokenString
        });
    }
}