using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;

public class ConnectionRequestParameters : IParameters
{
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
    public string? DepartureTime { get; set; }
    
    /// <summary>
    /// UTC Time of arrival
    /// </summary>
    /// <remarks>
    /// For setting departure time, please use <see cref="DepartureTime"/>
    /// </remarks>
    public string? ArrivalTime { get; set; }
    
    /// <summary>
    /// List of the passengers
    /// </summary>
    public List<PassengerParameters>? Passengers { get; set; }

    /// <summary>
    /// Comfort class of the trip
    /// </summary>
    /// <example>Second</example>
    public string? ComfortClass { get; set; }
    
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