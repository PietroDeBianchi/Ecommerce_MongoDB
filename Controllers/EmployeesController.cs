using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBTest.Models;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private IMongoCollection<Employee> _employees;

        // Constructor to initialize MongoDB connection
        public EmployeesController(IOptions<DbConfig> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(options.Value.DatabaseName);
            _employees = mongoDb.GetCollection<Employee>(options.Value.CollectionName);
        }

        // GET all employees
        [HttpGet]
        public async Task<List<Employee>> Get()
        {
            return await _employees.Find(e => true).ToListAsync();
        }

        // GET employee by ID
        [HttpGet("{id}")]
        public async Task<Employee> GetById(int id)
        {
            var employee = await _employees.Find(e => e.employeeNumber == id).FirstOrDefaultAsync();
            return employee;
        }

        // DELETE employee by ID
        [HttpDelete("{id}")]
        public async Task<DeleteResult> Delete(int id)
        {
            return await _employees.DeleteOneAsync(e => e.employeeNumber == id);
        }

        // POST new employee
        [HttpPost]
        public async Task<Employee> Create(Employee employee)
        {
            await _employees.InsertOneAsync(employee);
            return employee;
        }

        // PUT (update) employee by ID
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Employee employee)
        {
            Employee existingEmployee = await GetById(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Set the _id field in the replacement document
            employee.Id = existingEmployee.Id;

            // Pass the employee object directly to the ReplaceOneAsync operation
            await _employees.ReplaceOneAsync(e => e.employeeNumber == id, employee);

            return NoContent();
        }
    }
}
