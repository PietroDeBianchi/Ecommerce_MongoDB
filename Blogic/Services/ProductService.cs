// Import necessary namespaces
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;
using MongoDB.Bson;

namespace MongoDBTest.Blogic.Services;

public class ProductService
{
    // Declare private fields for MongoDB collections
    private readonly IMongoCollection<Product> _products;
    // Constructor to initialize MongoDB collections
    public ProductService(IMongoCollection<Product> products)
    {
        _products = products;
    }

    // Constructor to initialize MongoDB connection
    public ProductService(IOptions<DbConfig> options)
    {
        // Create a new MongoDB client
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        // Get the specified database
        var mongoDb = mongoClient.GetDatabase(options.Value.Databases.Ecommerce.Name);
        // Get the specified collection
        _products = mongoDb.GetCollection<Product>(options.Value.Databases.Ecommerce.Collections.Products);
    }

    // GET all products
    public async Task<List<Product>> GetAsync() => await _products.Find(p => true).ToListAsync();
    
    // GET product by ID
    public async Task<Product> GetIdAsync(string id) => await _products.Find(p => p.productCode == id).FirstOrDefaultAsync();

    // POST new product
    public async Task CreateAsync(Product product)
    {
        // Check if product already exists and throw exception if it does
        var existingProduct = await _products.Find(p => p.productCode == product.productCode).FirstOrDefaultAsync();
        if (existingProduct != null)
            throw new Exception("Product already exists");
        // Set default values for new product
        product.Id = ObjectId.GenerateNewId();
        // Insert new product
        await _products.InsertOneAsync(product);
    }

    // PUT (update) product by ID
    public async Task UpDateAsync(string id, Product product)
    {
        // Check if product already exists and throw exception if it does
        var existingProduct = await _products.Find(e => e.productCode == id).FirstOrDefaultAsync();
        if (existingProduct != null)
        {
            // Set default values for product
            product.Id = existingProduct.Id;
            product.productCode = existingProduct.productCode;
        }
        else
        {
            throw new Exception("Product not found");
        }
        // Update product
        await _products.ReplaceOneAsync(e => e.productCode == product.productCode, product);
    }

    // DELETE product by ID
    public async Task DeleteAsync(string id)
    {
        // Check if product already exists and throw exception if it does
        var product = await _products.Find(p => p.productCode == id).FirstOrDefaultAsync() ?? throw new Exception("Product not found");
        // Delete product
        await _products.DeleteOneAsync(e => e.productCode == id);
    }
}
