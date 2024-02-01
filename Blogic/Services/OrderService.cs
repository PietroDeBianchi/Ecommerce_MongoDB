using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;

namespace MongoDBTest.Blogic.Services;

public class OrderService
{
    // Declare private fields for MongoDB collections
    private readonly IMongoCollection<Order> _orders;

    // Constructor to initialize MongoDB connection
    public OrderService(IOptions<DbConfig> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(options.Value.DatabaseName);
        _orders = mongoDb.GetCollection<Order>(options.Value.CollectionOName);
    }

    // GET all orders
    public async Task<List<Order>> GetAsync() => await _orders.Find(e => true).ToListAsync();

    // GET order by ID
    public async Task<Order> GetIdAsync(int id) =>
        await _orders.Find(e => e.orderNumber == id).FirstOrDefaultAsync();

    // DELETE order by ID
    public async Task DeleteAsync(int id) =>
        await _orders.DeleteOneAsync(e => e.orderNumber == id);

    // POST new order
    public async Task CreateAsync(Order order) =>
        await _orders.InsertOneAsync(order);

    // PUT (update) order by ID
    public async Task UpDateAsync(Order order) =>
        await _orders.ReplaceOneAsync(e => e.orderNumber == order.orderNumber, order);

}