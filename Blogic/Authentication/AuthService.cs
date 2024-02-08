using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBTest.Models;

namespace MongoDBTest.Blogic.Authentication;

public class UserService {
    private readonly IMongoCollection<User> _users;
    public UserService(IOptions<DbConfig> settings) {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.Databases.Authentication.Name);
        _users = database.GetCollection<User>(settings.Value.Databases.Authentication.Collections.Users);
    }

    public async Task<List<User>> GetAsync() => await _users.Find(e => true).ToListAsync();

    public async Task<User> GetIdAsync(string id) => await _users.Find(u => u.Email == id).FirstOrDefaultAsync();

    public async Task<User> CreateAsync(User user)
    {
        // Check if product already exists and throw exception if it does
        var existingUser = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (existingUser != null)
            throw new Exception("User already exists");
       
       if (user.CustomerNumber != null) 
        {
            user.IsAdmin = false;
            user.EmployeeNumber = null;
        }            
        // Set default values for new product
        user.Id = ObjectId.GenerateNewId();
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        // Insert new product
        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User> LogInAsync(string email, string password)
    {
        var existingUser = await _users.Find(u => u.Email == email).FirstOrDefaultAsync() ?? throw new Exception("User does not exist");
        
        if (!BCrypt.Net.BCrypt.Verify(password, existingUser.Password))
            throw new Exception("Invalid password");
        return existingUser;
    }
}