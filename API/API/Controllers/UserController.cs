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
    public async Task<ActionResult<SignInReturnUserModel>> CreateUser([FromBody] SignUpUserModel model)
    {
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

        return new SignInReturnUserModel()
        {
            Username = model.Username,
            Email = model.Email
        };
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<SignInReturnUserModel>> LoginUser([FromBody] SignInUserModel signinmodel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users
            .Where(user => user.Email == signinmodel.Email)
            .FirstOrDefaultAsync();
        
        return new SignInReturnUserModel()
        {
            Username = user.Username,
            Email = user.Email
        };
    }
}