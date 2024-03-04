namespace API.Controllers.DTO.Users;

public class UpdateUserDto : BaseUserDto
{
    public int UserId { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }

    public string? Password { get; set; }
}
