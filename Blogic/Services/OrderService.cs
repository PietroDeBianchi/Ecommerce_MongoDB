using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;
using MongoDB.Bson;

namespace MongoDBTest.Blogic.Services;

public class OrderService
{
    // Declare private fields for MongoDB collections
    private readonly IMongoCollection<Order> _orders;

    // Constructor to initialize MongoDB connection
    public OrderService(IOptions<DbConfig> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(options.Value.Databases.Ecommerce.Name);
        _orders = mongoDb.GetCollection<Order>(options.Value.Databases.Ecommerce.Collections.Orders);
    }

    // GET all orders
    public async Task<List<Order>> GetAsync() 
    {
        // Define the lookup pipeline stage
        var lookupPipeline = new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "orderdetail" },
                { "localField", "orderNumber" },
                { "foreignField", "orderNumber" },
                { "as", "orderDetails" }
            });

        // Define the aggregation pipeline
        var pipeline = PipelineDefinition<Order, Order>.Create(new[]
        {
            lookupPipeline
        });

        // Execute the aggregation pipeline
        var orderWithDetails = await _orders.Aggregate(pipeline).ToListAsync();

        return orderWithDetails;
    }

    // GET order by ID
    public async Task<Order> GetIdAsync(long id)
    {
        // Define the match pipeline stage to filter by product code
        var matchPipeline = PipelineStageDefinitionBuilder.Match<Order>(o => o.orderNumber == id);

        // Define the lookup pipeline stage
        var lookupPipeline = new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "orderdetail" },
                { "localField", "orderNumber" },
                { "foreignField", "orderNumber" },
                { "as", "orderDetails" }
            });

        // Define the aggregation pipeline
        var pipeline = PipelineDefinition<Order, Order>.Create(new[]
        {
            matchPipeline,
            lookupPipeline
        });

        // Execute the aggregation pipeline and get the first matching product
        var orderWithDetails = await _orders.Aggregate(pipeline).FirstOrDefaultAsync();

        return orderWithDetails;
    }

    // POST new order
    public async Task CreateAsync(Order order) => 
        await _orders.InsertOneAsync(order);

    // PUT (update) order by ID
    public async Task UpDateAsync(Order order) => 
        await _orders.ReplaceOneAsync(e => e.orderNumber == order.orderNumber, order);

    // DELETE order by ID
    public async Task DeleteAsync(int id)
    {
        // Check if product already exists and throw exception if it does
        var product = await _orders.Find(e => e.orderNumber == id).FirstOrDefaultAsync() ?? throw new Exception("Order not found");
        // Delete product
        await _orders.DeleteOneAsync(e => e.orderNumber == id);
    }

}