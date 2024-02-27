using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Database.Models;

public class User
{
    public int UserId { get; set; }

    [Required]
    [StringLength(320)]
    public required string Email { get; set; }

    [StringLength(71)]
    public string? Password { get; set; }

    [Required]
    [StringLength(128)]
    public required string Name { get; set; }

    public void Sanitize()
    {
        Email = Email.Trim().ToLower();
    }

    public bool PasswordMatches(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Password);
    }

    public bool IsValidEmail()
    {
        var emailRegex = new Regex(@"^[\w\-\.]+@([\w-]+\.)+[\w-]{2,}$");

        return emailRegex.IsMatch(Email);
    }
}

