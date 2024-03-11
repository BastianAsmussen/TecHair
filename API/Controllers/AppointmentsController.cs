using API.Utility.Database.DAL;
using API.Utility.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new();

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments(
        [FromHeader(Name = "authorization")] string authorization)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var appointments = await _unitOfWork.AppointmentRepository.Get(includeProperties: "Barber,Customer");

            return Ok(appointments);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> PostAppointment(
        [FromHeader(Name = "authorization")] string authorization,
        [Bind("Date,Barber,Customer,Price,Notes")]
        Appointment appointment)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            // Make sure the employee exists.
            var barber = await _unitOfWork.EmployeeRepository.GetById(appointment.Barber.EmployeeId);
            if (barber == null) return BadRequest("Barber not found!");

            appointment.Barber = barber;

            // Make sure the customer exists.
            var customer = await _unitOfWork.UserRepository.GetById(appointment.Customer.UserId);
            if (customer == null) return BadRequest("Customer not found!");

            appointment.Customer = customer;

            _unitOfWork.AppointmentRepository.Insert(appointment);
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Appointment>> GetAppointment(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetById(id);
            if (appointment == null) return NotFound();

            return Ok(appointment);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AppointmentExists(id)) return NotFound();

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Appointment>> PutAppointment(
        [FromHeader(Name = "authorization")] string authorization,
        int id,
        [Bind("AppointmentId,Date,Barber,Customer,Price,Notes")]
        Appointment appointment)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        if (id != appointment.AppointmentId) return BadRequest();

        try
        {
            // Make sure the employee exists.
            var barber = await _unitOfWork.EmployeeRepository.GetById(appointment.Barber.EmployeeId);
            if (barber == null) return BadRequest("Barber not found!");

            appointment.Barber = barber;

            // Make sure the customer exists.
            var customer = await _unitOfWork.UserRepository.GetById(appointment.Customer.UserId);
            if (customer == null) return BadRequest("Customer not found!");

            appointment.Customer = customer;

            _unitOfWork.AppointmentRepository.Update(appointment);
            await _unitOfWork.Save();

            return Ok(appointment);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AppointmentExists(id)) return NotFound();

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointment(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetById(id);
            if (appointment == null) return NotFound();

            _unitOfWork.AppointmentRepository.Delete(appointment);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AppointmentExists(id)) return NotFound();

            throw;
        }
    }

    private async Task<bool> AppointmentExists(int id)
    {
        return await _unitOfWork.AppointmentRepository.GetById(id) != null;
    }
}