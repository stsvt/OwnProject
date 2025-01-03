using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class LoginReturnUserModel
{
    public required string Email { get; set; }
    public required string Username { get; set; }
}