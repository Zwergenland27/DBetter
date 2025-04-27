using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;

public class PassengerParameters : IParameters
{
    /// <summary>
    /// Identifier of the passenger
    /// </summary>
    /// <example></example>
    public string? Id { get; set; }

    /// <summary>
    /// Identifier of the user
    /// </summary>
    /// <remarks>
    /// Only set, if the passenger is a user
    /// !!Currently not supported!!
    /// </remarks>
    [JsonIgnore]
    public string? UserId { get; set; }

    /// <summary>
    /// Name of the passenger
    /// </summary>
    /// <example>Claire Grube</example>
    public string? Name { get; set; }

    /// <summary>
    /// Birthday of the passenger
    /// </summary>
    /// <remarks>
    /// Either birthday or age must be set
    /// </remarks>
    public string? Birthday { get; set; }
    
    /// <summary>
    /// Age of the passenger at the start of the trip
    /// </summary>
    /// <example>23</example>
    public int? Age { get; set; }

    /// <summary>
    /// Number of bikes the passenger will carry
    /// </summary>
    /// <example>0</example>
    public int? Bikes { get; set; }

    /// <summary>
    /// Number of dogs the passenger will carry
    /// </summary>
    /// <example>0</example>
    public int? Dogs { get; set; }
    
    /// <summary>
    /// Discount cards of the passenger
    /// </summary>
    public List<PassengerDiscountParameters>? Discounts { get; set; }
}