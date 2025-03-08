using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class ConnectionOptionsParameters : IParameters
{
    /// <summary>
    /// Class of the trip
    /// </summary>
    /// <example>Second</example>
    public string? Class;

    /// <summary>
    /// Maximum number of transfers on the connection
    /// </summary>
    /// <example>5</example>
    public int? MaxTransfers;

    /// <summary>
    /// Minimum transfer time for every transfer in minutes
    /// </summary>
    /// <example>10</example>
    public int? MinTransferMinutes;
}