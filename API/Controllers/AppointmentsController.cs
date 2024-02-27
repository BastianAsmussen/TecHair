using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointment()
    {
        return await context.Appointments
          .Select(a => a)
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
    {
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, appointment);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Appointment>> GetAppointment(long id)
    {
        var appointment = await context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        return appointment;
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Appointment>> PutAppointment(long id, Appointment appointment)
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

        foundAppointment = appointment;

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

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAppointment(long id)
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

    private bool AppointmentExists(long id)
    {
        return context.Appointments.Any(e => e.AppointmentId == id);
    }

}
