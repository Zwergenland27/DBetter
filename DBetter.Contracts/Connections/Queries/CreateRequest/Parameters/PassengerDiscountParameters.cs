using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.CreateRequest.Parameters;

public class PassengerDiscountParameters : IParameters
{
    /// <summary>
    /// Type of the Discount
    /// </summary>
    /// <example>BahnCard25</example>
    public string? Type { get; set; }
    
    /// <summary>
    /// Class where the discount is valid
    /// </summary>
    /// <example>Second</example>
    public string? ComfortClass { get; set; }
    
    /// <summary>
    /// Utc date of expiration
    /// </summary>
    public string? ValidUntil { get; set; }
}