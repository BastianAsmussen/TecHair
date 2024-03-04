using API.Controllers.DTO.Users;

namespace API.Controllers.DTO.Employees;

public class CreateEmployeeDto : BaseEmployeeDto
{
    public int? ManagerId { get; set; }
    public int UserId { get; set; }
}
