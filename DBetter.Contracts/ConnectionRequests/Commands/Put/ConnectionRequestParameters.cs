using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class ConnectionRequestParameters : IParameters
{
    /// <summary>
    /// Identifier of the request
    /// </summary>
    [JsonIgnore]
    public string? Id;

    /// <summary>
    /// Identifier of the user that created the request
    /// </summary>
    /// <remarks>
    /// Can be left empty if no user is logged in
    /// </remarks>
    public string? OwnerId;

    /// <summary>
    /// UTC Time of departure
    /// </summary>
    /// <remarks>
    /// For setting arrival time, please use <see cref="ArrivalTime"/>
    /// </remarks>
    public DateTime? DepartureTime;
    
    /// <summary>
    /// UTC Time of arrival
    /// </summary>
    /// <remarks>
    /// For setting departure time, please use <see cref="DepartureTime"/>
    /// </remarks>
    public DateTime? ArrivalTime;
    
    /// <summary>
    /// List of the passengers
    /// </summary>
    public List<PassengerParameters>? Passengers;

    /// <summary>
    /// Options for the connection
    /// </summary>
    public ConnectionOptionsParameters? Options;
    
    /// <summary>
    /// Requested route of the connection
    /// </summary>
    public ConnectionRouteParameters? Route;
}