using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.TrainRuns.Queries.GetRoute;

public class RouteStopResponse
{
    /// <summary>
    /// Internal id of the station
    /// </summary>
    public required string Id { get; init; }
    
    /// <summary>
    /// Eva number of the station
    /// </summary>
    public required string EvaNumber { get; init; }

    /// <summary>
    /// Departure time
    /// </summary>
    public required TravelTimeDto? DepartureTime { get; init; }
    
    /// <summary>
    /// Arrival time
    /// </summary>
    public required TravelTimeDto? ArrivalTime { get; init; }
    
    /// <summary>
    /// Platform
    /// </summary>
    public required PlatformDto? Platform { get; init; }
    
    /// <summary>
    /// Stop index of the full train run
    /// </summary>
    public required int RouteIndex { get; init; }
}