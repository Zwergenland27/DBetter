using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class PassengerOptionsParameters : IParameters
{
    /// <summary>
    /// Indicates that passenger wants to take a seat reservation
    /// </summary>
    public bool? Reservation { get; set; }

    /// <summary>
    /// Number of bikes the passenger will carry
    /// </summary>
    /// <example>0</example>
    public int Bikes { get; set; }

    /// <summary>
    /// Number of dogs the passenger will carry
    /// </summary>
    /// <example>0</example>
    public int? Dogs { get; set; }

    /// <summary>
    /// Indicates that the passenger needs accessibility
    /// </summary>
    public bool? NeedsAccessibility { get; set; }
}