using API.Controllers.DTO.Users;

namespace API.Controllers.DTO.Employees;

public class UpdateEmployeeDto : BaseEmployeeDto
{
    public int EmployeeId { get; set; }

    public UpdateUserDto User { get; set; }
}
