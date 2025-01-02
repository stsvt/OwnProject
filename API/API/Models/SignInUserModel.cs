using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class SignInUserModel
{
    [EmailAddress(ErrorMessage = "Invalid email")]
    public required string Email { get; set; }
    public required string Password { get; set; }
}