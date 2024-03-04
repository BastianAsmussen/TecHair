using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers.DTO.Employees;
using API.Controllers.DTO.Users;
using API.Utility.Database.Models;
using Mapster;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BaseEmployeeDto>>> GetEmployees() {
        return await context.Employees
          .Select(e => e.Adapt<BaseEmployeeDto>())
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<BaseEmployeeDto>> PostEmployee(CreateEmployeeDto employeeDto) {
        var manager = await context.Employees.FindAsync(employeeDto.ManagerId);
        if (manager == null) return BadRequest("Manager not found!");

        employeeDto.Manager = manager.Adapt<BaseEmployeeDto>();

        var user = await context.Users.FindAsync(employeeDto.UserId);
        if (user == null) return BadRequest("User not found!");

        employeeDto.User = user.Adapt<BaseUserDto>();

        var employee = employeeDto.Adapt<Employee>();
        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseEmployeeDto>> GetEmployee(int id)
    {
        var employee = await context.Employees.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return employee.Adapt<BaseEmployeeDto>();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseEmployeeDto>> PutEmployee(int id, UpdateEmployeeDto employee)
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

        var manager = await context.Employees.FindAsync(employee.Manager);
        if (manager == null)
        {
            return BadRequest("Manager not found!");
        }

        employee.Manager = manager.Adapt<BaseEmployeeDto>();

        var user = await context.Users.FindAsync(employee.User);
        if (user == null)
        {
            return BadRequest("User not found!");
        }

        employee.User = user.Adapt<UpdateUserDto>();
        foundEmployee = employee.Adapt(foundEmployee);
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

