using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;
using MongoDB.Bson;
using ZstdSharp;

namespace MongoDBTest.Blogic.Authentication;

public class UserService
{
    private readonly IMongoCollection<User> _users;
    // Constructor to initialize MongoDB collections
    public UserService(IMongoCollection<User> user)
    {
        _users = user;
    }

    public UserService(IOptions<DbConfig> options)
    {
        // Create a new MongoDB client
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(options.Value.Databases.Authentication.Name);
        // Get the specified collection
        _users = mongoDb.GetCollection<User>(options.Value.Databases.Authentication.Collections.Users);
    }

    // GET all users
    public async Task<List<User>> GetAsync() => await _users.Find(p => true).ToListAsync();
}