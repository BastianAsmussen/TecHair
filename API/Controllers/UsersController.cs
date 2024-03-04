using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers.DTO.Users;
using API.Utility.Database.Models;
using Mapster;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BaseUserDto>>> GetUsers() {
        return await context.Users
          .Select(u => u.Adapt<BaseUserDto>())
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<BaseUserDto>> PostUser(CreateUserDto userDto) {
        var user = userDto.Adapt<User>();
        user.Sanitize();

        if (!user.IsValidEmail())
        {
            return BadRequest("Invalid email!");
        }

        // Check if the email is already in use.
        var foundUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (foundUser != null)
        {
            return BadRequest("Email already in use!");
        }

        // Hash the password.
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseUserDto>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return user.Adapt<BaseUserDto>();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseUserDto>> PutUser(int id, UpdateUserDto user)
    {
        if (id != user.UserId)
        {
            return BadRequest();
        }

        var foundUser = await context.Users.FindAsync(id);
        if (foundUser == null)
        {
            return NotFound();
        }

        foundUser.Name = user.Name ?? foundUser.Name;
        foundUser.Email = user.Email ?? foundUser.Email;
        foundUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password) ?? foundUser.Password;
        foundUser.Sanitize();

        if (!foundUser.IsValidEmail())
        {
            return BadRequest("Invalid email!");
        }

        context.Users.Update(foundUser);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!UserExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool UserExists(int id)
    {
        return context.Users.Any(u => u.UserId == id);
    }
}
