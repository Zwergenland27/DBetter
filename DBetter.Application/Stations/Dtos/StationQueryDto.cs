using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations.Dtos;

public record StationQueryDto
{
    public required EvaNumber EvaNumber { get; init; }
    
    public required StationName Name { get; init; }
    
    public required Coordinates Location { get; init; }
}