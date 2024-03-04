using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers.DTO;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers() {
        return await context.Users
          .Select(u => new UserDto(u))
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> PostUser(UserDto user) {
        context.Users.Add(Utility.Database.Models.User.FromDto(user));
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return new UserDto(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> PutUser(int id, UserDto user)
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

        foundUser = Utility.Database.Models.User.FromDto(user);
        context.Users
            .Update(foundUser);

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

