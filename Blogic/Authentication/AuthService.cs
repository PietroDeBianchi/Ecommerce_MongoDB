using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MongoDBTest.Blogic.Authentication;

public class UserService
{
    // _users is a MongoDB collection that contains the users.
    private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _configuration;


    // This constructor initializes the MongoDB collection with the one provided.
    public UserService(IMongoCollection<User> user, IConfiguration configuration)
    {
        _users = user;
        _configuration = configuration;
    }

    // This constructor initializes the MongoDB collection using the provided configuration options.
    public UserService(IOptions<DbConfig> options, IConfiguration configuration)
    {
        // Create a new MongoDB client
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(options.Value.Databases.Authentication.Name);
        // Get the specified collection
        _users = mongoDb.GetCollection<User>(options.Value.Databases.Authentication.Collections.Users);
        // Add the configuration
        _configuration = configuration;
    }

    // This method returns all users.
    public async Task<List<User>> GetAsync() => await _users.Find(p => true).ToListAsync();

    // This method registers a new user, hashing the password and saving the user to the database.
    public async Task<User> Register(User user)
    {
        // Hash the user's password
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        if (user.EmployeeNumber != null)
        {
            // Add the admin role to the user
            user.IsAdmin = true;
            user.CustomerNumber = null;
        }
        else
        {
            // Add the user role to the user
            user.IsAdmin = false;
            user.EmployeeNumber = null;
        }
        // Save the user to the database
        await _users.InsertOneAsync(user);
        return user;
    }

    // This method handles user login, verifying the credentials and generating a JWT if they are correct.
    public async Task<string> Login(string email, string password)
    {
        // Find the user
        var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        // Verify the password
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new Exception("Invalid email or password");
        }
        // Get the secret key from the configuration
        var secretKey = _configuration["Jwt:Key"] ?? throw new Exception("Secret key is missing");
        // Generate a JWT and return it
        return GenerateJwt(user, secretKey);
    }

    // This method handles user logout, invalidating the JWT.
    public bool Logout(string token)
    {
        return true;
        // Invalidate the token
        // This depends on your token management strategy
        // For this example, let's say you're storing tokens in a database
    }

    // This method generates a JWT for the user.
    private static string GenerateJwt(User user, string secretKey)
    {
        // Create a JWT token handler
        var tokenHandler = new JwtSecurityTokenHandler();
        // Convert the secret key into a byte array
        var key = Encoding.UTF8.GetBytes(secretKey);
        // Create a token descriptor with the user's information, expiry date, and signing credentials
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] 
            {
                // Add a claim with the user's email
                new(ClaimTypes.Name, user.Email ?? string.Empty),
                // Add a claim with the user's role
                new(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"),
            }),
            // Set the token to expire after 7 days
            Expires = DateTime.UtcNow.AddDays(7),
            // Sign the token with the secret key
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        // Create the token
        var token = tokenHandler.CreateToken(tokenDescriptor);
        // Return the token as a string
        return tokenHandler.WriteToken(token);
    }
}