using System.Text.RegularExpressions;
using API.Data;
using API.Entities;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private readonly DataContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly IPasswordHasherService _passwordHasher;
    public UserController(DataContext context, ILogger<UserController> logger, IPasswordHasherService passwordHasher)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = passwordHasher;
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
        var isEmailExists = _context.Users.Any(u => u.Email == model.Email);
        var isUsernameExists = _context.Users.Any(u => u.Username == model.Username);
        
        if (isEmailExists || isUsernameExists)
            return BadRequest("User with this email or username is already existed.");
        
        if (!IsValidPassword(model.Password))
            return BadRequest("Password must be 8-20 characters and must include letters and digits.");
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var passwordHash = _passwordHasher.Hash(model.Password);
        
        var user = new User
        {
            Name = model.Name,
            Lastname = model.Lastname,
            Username = model.Username,
            Email = model.Email,
            Password = passwordHash 
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
            return BadRequest(ModelState);
        
        var user = await _context.Users
            .Where(user => user.Email == signinmodel.Email)
            .FirstOrDefaultAsync();

        var result = _passwordHasher.Verify(user.Password, signinmodel.Password);
        
        if (!result)
            return Unauthorized("Wrong email or password");

        return new LoginReturnUserModel()
        {
            Username = user.Username,
            Email = user.Email
        };
    }
    private bool IsValidPassword(string password)
    {
        if (password.Length < 8 || password.Length > 20)
            return false;
        
        if (!Regex.IsMatch(password, @"[a-zA-Z]") || !Regex.IsMatch(password, @"\d"))
            return false;
        
        return true;
    }
    
}