using System.Text.Json.Serialization;
using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Requests.Queries.GetSuggestions.Results;

/// <summary>
/// Stop of a train run
/// </summary>
public class StopResponse
{
    /// <summary>
    /// Internal id of the station
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Departure time
    /// </summary>
    public required TravelTimeDto? DepartureTime { get; set; }
    
    /// <summary>
    /// Arrival time
    /// </summary>
    public required TravelTimeDto? ArrivalTime { get; set; }
    
    /// <summary>
    /// Demand information
    /// </summary>
    public required DemandResponse Demand { get; set; }
    
    /// <summary>
    /// Name of the station
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string Name { get; set; }
    
    /// <summary>
    /// Ril100 identifier of station
    /// </summary>
    /// <example>DH</example>
    public required string? Ril100 { get; set; }
    
    /// <summary>
    /// Platform
    /// </summary>
    public required PlatformDto? Platform { get; set; }

        /// <summary>
    /// Indicates that the stop is additional to the default route
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public required bool IsAdditional { get; set; }

    /// <summary>
    /// Indicates that the stop has been cancelled
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public required bool IsCancelled { get; set; }

    /// <summary>
    /// Indicates that passengers are only allowed to leave the train at this stop
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public required bool IsExitOnly { get; set; }

    /// <summary>
    /// Indicates that passengers are only allowed to enter the train at this stop
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public required bool IsEntryOnly { get; set; }
    
    /// <summary>
    /// Stop index of the full train run
    /// </summary>
    public required int StopIndex { get; set; }
}