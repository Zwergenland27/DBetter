using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.Connections.Dtos;

public record StopDto
{
    public required TravelTime? DepartureTime { get; init; }
    
    public required TravelTime? ArrivalTime { get; init; }
    
    public required Demand Demand { get; init; }
    
    public required EvaNumber EvaNumber { get; init; }
    
    public required StationInfoId? InfoId { get; init; }
    
    public required StationName Name { get; init; }
    
    public required Platform? Platform { get; init; }
    
    public required StopAttributes Attributes { get; init; }
    
    public required StopIndex TrainRunIndex { get; init; }
}