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

    public async Task CreateAsync(User user)
    {
        // Check if product already exists and throw exception if it does
        var existingProduct = await _users.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
        if (user.CustomerNumber != null) 
            user.IsAdmin = false;
        // Set default values for new product
        user.Id = ObjectId.GenerateNewId();
        // Insert new product
        await _users.InsertOneAsync(user);
    }


}