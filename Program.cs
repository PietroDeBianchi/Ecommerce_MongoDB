using MongoDBTest.Models;
using MongoDBTest.Blogic.Services;
using MongoDBTest.Blogic.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

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

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        // options.SaveToken = true;
        // options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),
            RoleClaimType = ClaimTypes.Role
        };
    }
);
// Add services to enable API exploration and Swagger/OpenAPI generation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
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

// Becareful with the order of the following 3 lines
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Start the application
app.Run();