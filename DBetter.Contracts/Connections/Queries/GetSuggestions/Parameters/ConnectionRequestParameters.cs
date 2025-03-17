using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.ConnectionRequests.Commands.Put;

public class ConnectionRequestParameters : IParameters
{
    /// <summary>
    /// Identifier of the request
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Identifier of the user that created the request
    /// </summary>
    [JsonIgnore]
    public string? OwnerId { get; set; }

    /// <summary>
    /// UTC Time of departure
    /// </summary>
    /// <remarks>
    /// For setting arrival time, please use <see cref="ArrivalTime"/>
    /// </remarks>
    public DateTime? DepartureTime { get; set; }
    
    /// <summary>
    /// UTC Time of arrival
    /// </summary>
    /// <remarks>
    /// For setting departure time, please use <see cref="DepartureTime"/>
    /// </remarks>
    public DateTime? ArrivalTime { get; set; }
    
    /// <summary>
    /// List of the passengers
    /// </summary>
    public List<PassengerParameters>? Passengers { get; set; }

    /// <summary>
    /// Options for the connection
    /// </summary>
    public ConnectionOptionsParameters? Options { get; set; }
    
    /// <summary>
    /// Requested route of the connection
    /// </summary>
    public ConnectionRouteParameters? Route { get; set; }
    
    /// <summary>
    /// Pagination reference
    /// </summary>
    [JsonIgnore]
    public string? Page { get; set; }
}