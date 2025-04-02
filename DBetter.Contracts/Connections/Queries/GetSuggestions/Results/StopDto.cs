namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Stop of a train run
/// </summary>
public class StopDto
{
    /// <summary>
    /// Departure time
    /// </summary>
    public required TravelTime DepartureTime { get; set; }
    
    /// <summary>
    /// Arrival time
    /// </summary>
    public required TravelTime ArrivalTime { get; set; }
    
    /// <summary>
    /// Demand information
    /// </summary>
    public required DemandDto DemandDto { get; set; }
    
    /// <summary>
    /// Name of the station
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string Name { get; set; }
    
    /// <summary>
    /// Platform
    /// </summary>
    public required PlatformDto PlatformDto { get; set; }
    
    /// <summary>
    /// Stop index of the full train run
    /// </summary>
    public required int StopIndex { get; set; }
}