using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

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
    public DateTime? Birthday { get; set; }
    
    /// <summary>
    /// Age of the passenger at the start of the trip
    /// </summary>
    /// <example>23</example>
    public int? Age { get; set; }

    /// <summary>
    /// Individual options for the passenger
    /// </summary>
    public PassengerOptionsParameters? Options { get; set; }
    
    /// <summary>
    /// Discount cards of the passenger
    /// </summary>
    public List<PassengerDiscountParameters>? Discounts { get; set; }
}