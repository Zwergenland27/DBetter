using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations.Snapshots;

public record StationQuerySnapshot
{
    public required EvaNumber EvaNumber { get; init; }
    
    public required StationName Name { get; init; }
    
    public required Coordinates Location { get; init; }
}