using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;
using MongoDB.Bson;

namespace MongoDBTest.Services;

public class ProductService
{
    private readonly IMongoCollection<Product> _products;
    private readonly IMongoCollection<OrderDetail>? _orderDetails;
    public ProductService(IMongoCollection<Product> products, IMongoCollection<OrderDetail> orderDetails)
    {
        _products = products;
        _orderDetails = orderDetails;
    }

    // Constructor to initialize MongoDB connection
    public ProductService(IOptions<DbConfig> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(options.Value.DatabaseName);
        _products = mongoDb.GetCollection<Product>(options.Value.CollectionPName);
    }

    // GET all employees
    public async Task<List<Product>> GetAsync()
    {
        // Define the lookup pipeline stage
        var lookupPipeline = new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "orderdetail" },
                { "localField", "productCode" },
                { "foreignField", "productCode" },
                { "as", "orderDetails" }
            });

        // Define the aggregation pipeline
        var pipeline = PipelineDefinition<Product, Product>.Create(new[]
        {
            lookupPipeline
        });

        // Execute the aggregation pipeline
        var productsWithOrderDetails = await _products.Aggregate(pipeline).ToListAsync();

        return productsWithOrderDetails;
    }

    // GET employee by ID
    public async Task<Product> GetIdAsync(string id)
    {
        // Define the match pipeline stage to filter by product code
        var matchPipeline = PipelineStageDefinitionBuilder.Match<Product>(p => p.productCode == id);

        // Define the lookup pipeline stage
        var lookupPipeline = new BsonDocument("$lookup",
            new BsonDocument
            {
                { "from", "orderdetail" },
                { "localField", "productCode" },
                { "foreignField", "productCode" },
                { "as", "orderDetails" }
            });

        // Define the aggregation pipeline
        var pipeline = PipelineDefinition<Product, Product>.Create(new[]
        {
            matchPipeline,
            lookupPipeline
        });

        // Execute the aggregation pipeline and get the first matching product
        var productWithOrderDetails = await _products.Aggregate(pipeline).FirstOrDefaultAsync();

        return productWithOrderDetails;
    }




    // DELETE employee by ID
    public async Task DeleteAsync(string id) =>
        await  _products.DeleteOneAsync(e => e.productCode == id);

    // POST new employee
    public async Task CreateAsync(Product product) =>
        await  _products.InsertOneAsync(product);

    // PUT (update) employee by ID
    public async Task UpDateAsync(Product product) =>
        await  _products.ReplaceOneAsync(e => e.productCode == product.productCode, product);

}