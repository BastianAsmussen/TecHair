using API.Utility.Database.Models;

namespace API.Controllers.DTO;

public class AppointmentDto(int id, DateTime date, AppointmentStatus status, EmployeeDto barber, UserDto customer, decimal price, string? notes = null)
{
    public int AppointmentId { get; } = id;

    public DateTime Date { get; } = date;

    public AppointmentStatus Status { get; } = status;

    public EmployeeDto Barber { get; } = barber;
    public UserDto Customer { get; } = customer;

    public decimal Price { get; } = price;
    public string? Notes { get; } = notes;

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;

    public AppointmentDto(Appointment appointment) : this(appointment.AppointmentId, appointment.Date, appointment.Status, new(appointment.Barber), new(appointment.Customer), appointment.Price, appointment.Notes) { }
}
