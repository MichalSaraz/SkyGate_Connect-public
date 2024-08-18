using System.ComponentModel.DataAnnotations;
using Core.Identity.Enums;
using Xunit.Sdk;

namespace Web.Api.AccountManagement.Models;

public class RegisterModel
{
    [Required]
    public required string UserName { get; set; }
    
    [Required]
    public required RoleEnum Role { get; set; }
    
    [Required]
    public required string Station { get; set; }
    
    [Required]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        ErrorMessage = "Password must contain at least 8 characters, one uppercase letter, one number, and one special character.")]
    public required string Password { get; set; }
}