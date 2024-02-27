using Database.Models;
using API.Controllers.Users;

namespace API.Controllers.Employees;

public class SimpleEmployee
{
    public int EmployeeId { get; set; }

    public SimpleEmployee? Manager { get; set; }
    public SimpleUser User { get; set; }

    public SimpleEmployee(Employee employee)
    {
        EmployeeId = employee.EmployeeId;

        Manager = employee.Manager == null ? null : new SimpleEmployee(employee.Manager);
        User = new SimpleUser(employee.User);
    }
}

