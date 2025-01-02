using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class SignUpUserModel
{
    
    public required string Name { get; set; }

    public required string Lastname { get; set; }
    public required string Username { get; set; }
    
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public required string Email { get; set; }

    public required string Password { get; set; }
    
}