using API.Utility.Database.DAL;
using API.Utility.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new();

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees(
        [FromHeader(Name = "authorization")] string authorization)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var employees = await _unitOfWork.EmployeeRepository.Get();

            return Ok(employees);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee(
        [FromHeader(Name = "authorization")] string authorization,
        [Bind("Manager,User")] Employee employee)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            if (employee.Manager != null)
            {
                var manager = await _unitOfWork.EmployeeRepository.GetById(employee.Manager.EmployeeId);
                if (manager == null) return BadRequest("Manager not found!");

                employee.Manager = manager;
            }

            var user = await _unitOfWork.UserRepository.GetById(employee.User.UserId);
            if (user == null) return BadRequest("User not found!");

            employee.User = user;

            _unitOfWork.EmployeeRepository.Insert(employee);
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Employee>> GetEmployee(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
            if (employee == null) return NotFound();

            return Ok(employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EmployeeExists(id)) return NotFound();

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Employee>> PutEmployee(
        [FromHeader(Name = "authorization")] string authorization,
        int id,
        [Bind("EmployeeId,Manager,User")] Employee employee)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        if (id != employee.EmployeeId) return BadRequest();

        try
        {
            if (employee.Manager != null)
            {
                var manager = await _unitOfWork.EmployeeRepository.GetById(employee.Manager.EmployeeId);
                if (manager == null) return BadRequest("Manager not found!");

                employee.Manager = manager;
            }

            var user = await _unitOfWork.UserRepository.GetById(employee.User.UserId);
            if (user == null) return BadRequest("User not found!");

            employee.User = user;

            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.Save();

            return Ok(employee);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EmployeeExists(id)) return NotFound();

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(id);
            if (employee == null) return NotFound();

            _unitOfWork.EmployeeRepository.Delete(employee);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await EmployeeExists(id)) return NotFound();

            throw;
        }
    }

    private async Task<bool> EmployeeExists(int id)
    {
        return await _unitOfWork.EmployeeRepository.Get(e => e.EmployeeId == id) != null;
    }
}