using API.Utility.Database.Models;
using API.Controllers.DTO.Users;
using API.Controllers.DTO.Employees;

namespace API.Controllers.DTO.Appointments;

public class BaseAppointmentDto
{
    public int AppointmentId { get; set; }

    public DateTime Date { get; set; }
    public AppointmentStatus Status { get; set; }

    public BaseEmployeeDto Barber { get; set; }
    public BaseUserDto Customer { get; set; }

    public decimal Price { get; set; }
    public string? Notes { get; set; }
}
