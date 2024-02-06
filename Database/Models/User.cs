using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
}
