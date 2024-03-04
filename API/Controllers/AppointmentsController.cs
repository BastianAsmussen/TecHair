using API.Controllers.DTO.Appointments;
using API.Controllers.DTO.Employees;
using API.Controllers.DTO.Users;
using API.Utility.Database.Models;
using Database;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BaseAppointmentDto>>> GetAppointment()
    {
        return await context.Appointments
          .Select(a => a.Adapt<BaseAppointmentDto>())
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<BaseAppointmentDto>> PostAppointment(CreateAppointmentDto appointment)
    {
        if (appointment.Date < DateTime.Now) return BadRequest("Invalid date!");

        var barber = await context.Employees.FindAsync(appointment.BarberId);
        if (barber == null) return BadRequest("Barber not found!");

        appointment.Barber = barber.Adapt<BaseEmployeeDto>();

        var customer = await context.Users.FindAsync(appointment.CustomerId);
        if (customer == null) return BadRequest("Customer not found!");

        appointment.Customer = customer.Adapt<BaseUserDto>();

        context.Appointments.Add(appointment.Adapt<Appointment>());
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseAppointmentDto>> GetAppointment(int id)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        return appointment.Adapt<BaseAppointmentDto>();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseAppointmentDto>> PutAppointment(int id, UpdateAppointmentDto appointment)
    {
        if (id != appointment.AppointmentId)
        {
            return BadRequest();
        }

        var foundAppointment = await context.Appointments.FindAsync(id);
        if (foundAppointment == null)
        {
            return NotFound();
        }

        foundAppointment = appointment.Adapt(foundAppointment);
        context.Appointments
            .Update(foundAppointment);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!AppointmentExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool AppointmentExists(int id)
    {
        return context.Appointments.Any(a => a.AppointmentId == id);
    }

}
