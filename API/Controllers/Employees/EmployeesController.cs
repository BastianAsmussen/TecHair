using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Utility;

namespace API.Controllers.Employees;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SimpleEmployee>>> GetEmployees()
    {
        return await context.Employees
          .Select(e => new SimpleEmployee(e))
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<SimpleEmployee>> PostEmployee(Employee employee)
    {
        // Sanitize the user.
        employee.User.Sanitize();
        if (!employee.User.IsValidEmail())
        {
            return BadRequest("Invalid email!");
        }

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, new SimpleEmployee(employee));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SimpleEmployee>> GetEmployee(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return new SimpleEmployee(employee);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SimpleEmployee>> PutEmployee(int id, Employee employee)
    {
        if (id != employee.EmployeeId)
        {
            return BadRequest("ID mismatch!");
        }

        // Sanitize the user.
        employee.User.Sanitize();
        if (!employee.User.IsValidEmail())
        {
            return BadRequest("Invalid email!");
        }

        var foundEmployee = await context.Employees.FindAsync(id);
        if (foundEmployee == null)
        {
            return NotFound();
        }

        foundEmployee = employee;
        context.Employees.Update(foundEmployee);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!EmployeeExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        context.Employees.Remove(employee);
        await context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool EmployeeExists(int id)
    {
        return context.Employees.Any(e => e.EmployeeId == id);
    }
}

