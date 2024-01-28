using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MongoDBTest.Models;

namespace MongoDBTest.Services;

public class EmployeeService
{
    private readonly IMongoCollection<Employee> _employees;

    // Constructor to initialize MongoDB connection
    public EmployeeService(IOptions<DbConfig> options)
    {
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(options.Value.DatabaseName);
        _employees = mongoDb.GetCollection<Employee>(options.Value.CollectionName);
    }

    // GET all employees
    public async Task<List<Employee>> GetAsync() => await _employees.Find(e => true).ToListAsync();

    // GET employee by ID
    public async Task<Employee> GetIdAsync(int id) =>
        await _employees.Find(e => e.employeeNumber == id).FirstOrDefaultAsync();

    // DELETE employee by ID
    public async Task DeleteAsync(int id) =>
        await _employees.DeleteOneAsync(e => e.employeeNumber == id);

    // POST new employee
    public async Task CreateAsync(Employee employee) =>
        await _employees.InsertOneAsync(employee);

    // PUT (update) employee by ID
    public async Task UpDateAsync(Employee employee) =>
        await _employees.ReplaceOneAsync(e => e.employeeNumber == employee.employeeNumber, employee);

}