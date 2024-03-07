namespace API.Utility.Database.Models;

public class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime Date { get; set; }
    public AppointmentStatus Status { get; set; }

    public Employee Barber { get; set; }
    public User Customer { get; set; }

    public decimal Price { get; set; }
    public string? Notes { get; set; }

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;
}

public enum AppointmentStatus
{
    Pending,
    Cancelled,
    Completed
}