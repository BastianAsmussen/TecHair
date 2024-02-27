using Database.Models;

namespace API.Controllers.Users;

public class SimpleUser
{
    public int UserId { get; set; }

    public string Email { get; set; }
    public string Name { get; set; }

    public SimpleUser(User user)
    {
        UserId = user.UserId;

        Email = user.Email;
        Name = user.Name;
    }
}

