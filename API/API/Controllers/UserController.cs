using System.Text.RegularExpressions;
using API.Data;
using API.Entities;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private readonly DataContext _context;
    
    public UserController(DataContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUser()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult<LoginReturnUserModel>> CreateUser([FromBody] RegisterUserModel model)
    {
        if (!IsValidPassword(model.Password))
        {
            return BadRequest("Password must be 8-20 characters and must include letters and digits.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var user = new User
        {
            Name = model.Name,
            Lastname = model.Lastname,
            Username = model.Username,
            Email = model.Email,
            Password = model.Password

        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new LoginReturnUserModel()
        {
            Username = model.Username,
            Email = model.Email
        };
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<LoginReturnUserModel>> LoginUser([FromBody] LoginUserModel signinmodel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users
            .Where(user => user.Email == signinmodel.Email)
            .FirstOrDefaultAsync();
        
        return new LoginReturnUserModel()
        {
            Username = user.Username,
            Email = user.Email
        };
    }

    private bool IsValidPassword(string password)
    {
        if (password.Length < 8 || password.Length > 20)
        {
            return false;
        }
        
        if (!Regex.IsMatch(password, @"a-zA-Z") || !Regex.IsMatch(password, @"\d"))
        {
            return false;
        }

        return true;
    }
}