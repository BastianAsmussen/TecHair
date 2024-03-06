using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Utility.Database.DAL;
using API.Utility.Database.Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new();

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers([FromHeader(Name = "authorization")] string authorization)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try {
            var users = await _unitOfWork.UserRepository.Get();

            return Ok(users);
        } catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<User>> PostUser(
        [FromHeader(Name = "authorization")] string authorization,
        [Bind("Name,Email,Password")]
        User user) {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        user.Sanitize();

        if (!user.IsValidEmail()) return BadRequest("Invalid email!");

        try
        {
            // Check if the email is already in use.
            var foundUser = await _unitOfWork.UserRepository.Get(filter: u => u.Email == user.Email);
            if (foundUser != null && foundUser.Any())
            {
                return BadRequest("Email already in use!");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        } catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        } catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
            {
                return NotFound();
            }

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> PutUser(
        [FromHeader(Name = "authorization")] string authorization,
        int id,
        [Bind("UserId,Name,Email,Password")]
        User user)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        if (id != user.UserId)
        {
            return BadRequest();
        }

        user.Sanitize();

        if (!user.IsValidEmail())
        {
            return BadRequest("Invalid email!");
        }

        try
        {
            // Check if the email is already in use.
            var foundUser = await _unitOfWork.UserRepository.Get(filter: u => u.Email == user.Email);
            if (foundUser != null)
            {
                return BadRequest("Email already in use!");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Save();

            return Ok(user);
        } catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
            {
                return NotFound();
            }

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.Save();

            return NoContent();
        } catch (DbUpdateConcurrencyException)
        {
            if (!await UserExists(id))
            {
                return NotFound();
            }

            throw;
        }
    }

    [HttpGet("login")]
    public async Task<ActionResult<Authorization>> Login(
        [FromBody]
        [Bind("Email,Password")]
        User user)
    {
        var foundUsers = await _unitOfWork.UserRepository.Get(filter: u => u.Email == user.Email);
        if (foundUsers == null || !foundUsers.Any())
        {
            return NotFound();
        }

        var foundUser = foundUsers.First();
        if (!BCrypt.Net.BCrypt.Verify(user.Password, foundUser.Password))
        {
            return Unauthorized();
        }

        return new Authorization { Role = Role.User, Token = Authorization.GenerateToken(foundUser), User = foundUser };
    }

    private async Task<bool> UserExists(int id)
    {
        return await _unitOfWork.UserRepository.Get(filter: u => u.UserId == id) != null;
    }
}
