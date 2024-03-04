using API.Controllers.DTO;
using API.Utility.Database.Models;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointment()
    {
        return await context.Appointments
          .Select(a => new AppointmentDto(a))
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> PostAppointment(AppointmentDto appointment)
    {
        context.Appointments.Add(Appointment.FromDto(appointment));
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int id)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        return new AppointmentDto(appointment);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> PutAppointment(int id, AppointmentDto appointment)
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

        foundAppointment = Appointment.FromDto(appointment);

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
