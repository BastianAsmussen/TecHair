using API.Utility.Database.Models;

namespace API.Controllers.DTO;

public class UserAuthorization
{
    public Role Role { get; set; }
    public string Token { get; set; }
}
