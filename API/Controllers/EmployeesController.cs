using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers.DTO;
using API.Utility.Database.Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees() {
        return await context.Employees
          .Select(e => new EmployeeDto(e))
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> PostEmployee(EmployeeDto employee) {
        context.Employees.Add(Employee.FromDto(employee));
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return new EmployeeDto(employee);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> PutEmployee(int id, EmployeeDto employee)
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

        foundEmployee = Employee.FromDto(employee);

        context.Employees
            .Update(foundEmployee);

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

