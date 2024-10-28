using System.ComponentModel.DataAnnotations;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Users.Commands;

public class RegisterUserParameters : IParameters
{
    /// <summary>
    /// First name of the user
    /// </summary>
    /// <example>Max</example>
    [Required]
    public string? Firstname { get; set; }
    
    /// <summary>
    /// Last name of the user
    /// </summary>
    /// <example>Mustermann</example>
    [Required]
    public string? Lastname { get; set; }
    
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