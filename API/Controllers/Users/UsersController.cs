using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SimpleUser>>> GetUsers() {
        return await context.Users
          .Select(u => new SimpleUser(u))
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<SimpleUser>> PostUser(User user) {
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SimpleUser>> GetUser(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return new SimpleUser(user);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<SimpleUser>> PutUser(int id, User user)
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

        foundUser = user;
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

