namespace API.Controllers.DTO.Users;

public class CreateUserDto : BaseUserDto
{
    public string Name { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }
}
