using API.Utility.Database.Models;

namespace API.Controllers.DTO;

public class NewUser
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public Role Role { get; set; }
}
