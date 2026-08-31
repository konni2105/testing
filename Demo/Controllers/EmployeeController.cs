using Demo.Data;
using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    //ControllerBase-> class provided by ASP.NET Core for creating Web API controllers.
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }



        // GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();//select * from Employees

            return Ok(employees);
        }

        // GET: api/Employee/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();//async allows the server to handle other requests instead of blocking the thread.

            return Ok(employee);
        }

        // PUT: api/Employee/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
            {
                return NotFound();
            }

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Department = employee.Department;
            existingEmployee.Salary = employee.Salary;

            await _context.SaveChangesAsync();//insert into tble vaues

            return Ok(existingEmployee);
        }

        // DELETE: api/Employee/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return Ok("Employee deleted successfully.");
        }
    }
}