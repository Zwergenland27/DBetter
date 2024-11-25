using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;

public class RefreshJwtTokenParameters : IParameters
{
    /// <summary>
    /// Id of the user
    /// </summary>
    [Required]
    public string? Id { get; set; }
    
    /// <summary>
    /// Refresh token to refresh the jwt token
    /// </summary>
    [JsonIgnore]
    public string? RefreshToken { get; set; }
}