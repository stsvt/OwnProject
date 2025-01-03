using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class LoginUserModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]

    public string Password { get; set; }
}