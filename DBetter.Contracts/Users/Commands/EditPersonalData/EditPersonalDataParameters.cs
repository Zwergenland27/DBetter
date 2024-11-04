using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Users.Commands.EditPersonalData;

public class EditPersonalDataParameters : IParameters
{
    /// <summary>
    /// Id of the user
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }
    
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
    /// Birthday of the user
    /// </summary>
    [Required]
    public DateTime? Birthday { get; set; }
}