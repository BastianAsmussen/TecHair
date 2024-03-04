using API.Utility.Database.Models;

namespace API.Controllers.DTO;

public class UserDto(int id, string email, string name)
{
    public int UserId { get; } = id;

    public string Email { get; } = email;
    public string Name { get; } = name;

    public UserDto(User user) : this(user.UserId, user.Email, user.Name) { }
}
