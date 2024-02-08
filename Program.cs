// Import necessary namespaces
using MongoDBTest.Models;
using MongoDBTest.Blogic.Services;

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
// https://shahedbd.medium.com/net-7-web-api-jwt-authentication-and-role-based-authorization-f2ff81f69cd4
// https://www.youtube.com/watch?v=9IBNIbgMGdM&t=2s

// Add services to enable API exploration and Swagger/OpenAPI generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// to add for CORS Policy access!
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .AllowAnyHeader());
});
// Build the application
var app = builder.Build();

// Use the CORS policy
app.UseCors("CorsPolicy");

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

// Map controller routes
app.MapControllers();

// Start the application
app.Run();