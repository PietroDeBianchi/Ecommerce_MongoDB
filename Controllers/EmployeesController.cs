using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBTest.Blogic.Services;

namespace MongoDB.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    // EmployeeService instance
    private readonly EmployeeService _employeeService;

    // Constructor for the EmployeesController
    public EmployeesController(EmployeeService employeeService)
    {
        // If employeeService is null, throw an exception
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    // HTTP GET method to get a list of all employees
    [Authorize(Roles = "Admin")] // Apply authorization to the entire controller
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Get all employees from the service
            var allEmployees = await _employeeService.GetAsync();

            // If no employees are found, return No Content status
            if (allEmployees == null)
                return NoContent(); // 204 No Content

            // If employees are found, return them with OK status
            return Ok(allEmployees);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // HTTP GET method to get an employee by its ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            // Get the employee from the service
            var employee = await _employeeService.GetIdAsync(id);

            // If the employee is not found, return Not Found status
            if (employee == null)
                return NotFound(); // 404 Not Found

            // If the employee is found, return it with OK status
            return Ok(employee);
        }
        catch (Exception ex)
        {
            // If an error occurs, return Internal Server Error status with the error message
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}



