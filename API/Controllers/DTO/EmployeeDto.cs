using API.Utility.Database.Models;

namespace API.Controllers.DTO;

public class EmployeeDto(int id, EmployeeDto? manager, UserDto user)
{
    public int EmployeeId { get; } = id;

    public EmployeeDto? Manager { get; } = manager;
    public UserDto User { get; } = user;

    public EmployeeDto(Employee employee) : this(employee.EmployeeId, employee.Manager is null ? null : new(employee.Manager), new(employee.User)) { }
}
