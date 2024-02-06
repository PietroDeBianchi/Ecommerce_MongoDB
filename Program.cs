// Import necessary namespaces
using MongoDBTest.Models;
using MongoDBTest.Blogic.Services;
using MongoDBTest.Blogic.Authentication;

// Create a new WebApplication builder with the provided command-line arguments
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add the MVC Controllers service to the DI container
builder.Services.AddControllers();

// Configure MongoDB settings
// Get the MongoDB configuration from appsettings.json and bind it to DbConfig
builder.Services.Configure<DbConfig>(builder.Configuration.GetSection("MongoDBconfig"));

// Configure Collections services
// Register the EmployeeService, ProductService, and OrderService as singletons in the DI container
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<UserService>();

// Add services to enable API exploration and Swagger/OpenAPI generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline.
// If the application is in the development environment
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI for development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Use the authorization middleware in the pipeline
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Start the application
app.Run();