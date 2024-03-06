using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Utility.Database.DAL;
using Microsoft.Build.Framework;
using Microsoft.IdentityModel.Tokens;

namespace API.Utility.Database.Models;

public class Authorization
{
    public int AuthorizationId { get; set; }

    [Required]
    public string Token { get; set; }

    [Required]
    public Role Role { get; set; }
    [Required]
    public User User { get; set; }

    public static async Task<Authorization?> Validate(UnitOfWork unitOfWork, string token, Role requiredRole)
    {
        if (token.IsNullOrEmpty() || !token.StartsWith("Bearer "))
            return null;

        // Remove the "Bearer " prefix and any leading/trailing whitespace.
        token = token.Replace("Bearer ", "").Trim();

        // Allow the secret to be used as a token for testing.
        if (token == Program.Settings.Secret) return new Authorization { Role = Role.Admin, Token = token, User = new User { UserId = 0 } };

        var results = await unitOfWork.AuthorizationRepository.Get(filter: a => a.Token == token);
        if (results == null || !results.Any())
            return null;

        var auth = results.First();
        return auth.Role != requiredRole ? null : auth;
    }

    public static string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(user.Password!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserId.ToString()) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public enum Role
{
    Admin,
    User
}
