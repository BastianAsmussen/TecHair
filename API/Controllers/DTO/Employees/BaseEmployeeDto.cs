using API.Controllers.DTO.Users;

namespace API.Controllers.DTO.Employees;

public class BaseEmployeeDto
{
    public int EmployeeId { get; set; }

    public BaseEmployeeDto? Manager { get; set; }
    public BaseUserDto User { get; set; }
}
