using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Requests.Snapshots;

public record StopSnapshot
{
    public required TravelTime? DepartureTime { get; init; }
    
    public required TravelTime? ArrivalTime { get; init; }
    
    public required Demand Demand { get; init; }
    
    public required EvaNumber EvaNumber { get; init; }
    
    public required StationInfoId? InfoId { get; init; }
    
    public required StationName Name { get; init; }
    
    public required Platform? Platform { get; init; }
    
    public required StopAttributes Attributes { get; init; }
    
    public required StopIndex RouteIndex { get; init; }
}