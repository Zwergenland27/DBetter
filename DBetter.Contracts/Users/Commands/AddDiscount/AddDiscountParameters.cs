using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Users.Commands.AddDiscount;

public class AddDiscountParameters : IParameters
{
    /// <summary>
    /// Id of the user
    /// </summary>
    [JsonIgnore]
    public string? UserId { get; set; }
    
    /// <summary>
    /// Type of the Discount
    /// </summary>
    /// <example>BahnCard25</example>
    public string? Type { get; set; }
    
    /// <summary>
    /// Class where the discount is valid
    /// </summary>
    /// <example>Second</example>
    public string? Class { get; set; }
    
    /// <summary>
    /// Utc date of the 
    /// </summary>
    public DateTime? BoughtAt { get; set; }
    
    /// <summary>
    /// Utc date of expiration
    /// </summary>
    public DateTime? ValidUntil { get; set; }
}