namespace API.Utility.Database.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public Employee? Manager { get; set; }
    public User User { get; set; }
}
