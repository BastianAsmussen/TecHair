using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees() {
        return await context.Employees
          .Select(e => e)
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(Employee employee) {
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Employee>> GetEmployee(long id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return employee;
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Employee>> PutEmployee(long id, Employee employee)
    {
        if (id != employee.EmployeeId)
        {
            return BadRequest();
        }

        var foundEmployee = await context.Employees.FindAsync(id);
        if (foundEmployee == null)
        {
            return NotFound();
        }

        foundEmployee = employee;

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

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteEmployee(long id)
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

    private bool EmployeeExists(long id)
    {
        return context.Employees.Any(e => e.EmployeeId == id);
    }
}

