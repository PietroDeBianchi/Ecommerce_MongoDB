using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;
using MongoDB.Bson;
using ZstdSharp;

namespace MongoDBTest.Blogic.Authentication;

public class UserService
{
    private readonly IMongoCollection<User> _users;

     public UserService(IOptions<DbConfig> options)
    {
        // Create a new MongoDB client
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(options.Value.DatabaseName);
        // Get the specified collection
        _users = mongoDb.GetCollection<User>(options.Value.CollectionPName);
    }

    public User Register(User user)
    {
        // Hash the password before saving the user to the database
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _users.InsertOne(user);
        return user;
    }

    public User Authenticate(string email, string password)
    {
        var user = _users.Find(user => user.Email == email).FirstOrDefault();

        // Verify the password
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new Exception("User not found o data invalid"); ;
        }

        return user;
    }
}