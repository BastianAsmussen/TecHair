using API.Controllers.DTO;

namespace API.Utility.Database.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public Employee? Manager { get; set; }
    public User User { get; set; }

    public static Employee FromDto(EmployeeDto dto)
    {
        return new Employee
        {
            EmployeeId = dto.EmployeeId,

            Manager = dto.Manager is null ? null : FromDto(dto.Manager),
            User = User.FromDto(dto.User)
        };
    }
}
