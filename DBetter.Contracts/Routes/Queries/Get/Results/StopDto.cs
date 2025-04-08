using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Routes.Queries.Get.Results;

/// <summary>
/// A stop on a route
/// </summary>
public class StopDto
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
    public required DemandDto Demand { get; set; }
    
    /// <summary>
    /// Name of the station
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string Name { get; set; }
    
    /// <summary>
    /// Platform
    /// </summary>
    public required PlatformDto? Platform { get; set; }
    
    /// <summary>
    /// Stop index of the full train run
    /// </summary>
    public required int StopIndex { get; set; }
}