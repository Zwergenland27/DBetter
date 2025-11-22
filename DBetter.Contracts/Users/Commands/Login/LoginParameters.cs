using System.ComponentModel.DataAnnotations;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Users.Commands.Login;

public class LoginParameters : IParameters
{
    /// <summary>
    /// Email of the user
    /// </summary>
    /// <example>max.mustermann@gmail.com</example>
    [Required]
    public string? Email { get; set; }
    
    /// <summary>
    /// Password of the user
    /// </summary>
    /// <example>super-secret-password</example>
    [Required]
    public string? Password { get; set; }
}