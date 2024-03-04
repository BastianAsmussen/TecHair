using API.Controllers.DTO;

namespace API.Utility.Database.Models;

public class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime Date { get; set;  }
    public AppointmentStatus Status { get; set; }

    public Employee Barber { get; set; }
    public User Customer { get; set;  }

    public decimal Price { get; set; }
    public string? Notes { get; set; }

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;

    public static Appointment FromDto(AppointmentDto appointment)
    {
        return new Appointment
        {
            AppointmentId = appointment.AppointmentId,

            Date = appointment.Date,
            Status = appointment.Status,

            Barber = Employee.FromDto(appointment.Barber),
            Customer = User.FromDto(appointment.Customer),

            Price = appointment.Price,
            Notes = appointment.Notes
        };
    }
}

public enum AppointmentStatus
{
    Pending,
    Cancelled,
    Completed
}
