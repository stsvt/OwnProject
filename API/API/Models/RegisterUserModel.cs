using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class RegisterUserModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    

    [Required(ErrorMessage = "Lastname is required")]
    public string Lastname { get; set; }
    
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
}