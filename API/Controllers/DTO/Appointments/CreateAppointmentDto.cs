using API.Utility.Database.Models;

namespace API.Controllers.DTO.Appointments;

public class CreateAppointmentDto : BaseAppointmentDto
{
    public DateTime Date { get; set; }
    public AppointmentStatus Status { get; set; }

    public int BarberId { get; set; }
    public int CustomerId { get; set; }

    public decimal Price { get; set; }
    public string? Notes { get; set; }
}
