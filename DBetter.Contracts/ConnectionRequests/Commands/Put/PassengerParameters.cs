using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class PassengerParameters : IParameters
{
    /// <summary>
    /// Identifier of the passenger
    /// </summary>
    /// <example></example>
    public string? Id;

    /// <summary>
    /// Identifier of the user
    /// </summary>
    /// <remarks>
    /// Only set, if the passenger is a user
    /// </remarks>
    public string? UserId;

    /// <summary>
    /// Name of the passenger
    /// </summary>
    /// <example>Claire Grube</example>
    public string? Name;

    /// <summary>
    /// Birthday of the passenger
    /// </summary>
    /// <remarks>
    /// Either birthday or age must be set
    /// </remarks>
    public DateTime? Birthday;
    
    /// <summary>
    /// Age of the passenger at the start of the trip
    /// </summary>
    /// <example>23</example>
    public int? Age;

    /// <summary>
    /// Individual options for the passenger
    /// </summary>
    public PassengerOptionsParameters? Options;
    
    /// <summary>
    /// Discount cards of the passenger
    /// </summary>
    public List<PassengerDiscountParameters>? Discounts;
}