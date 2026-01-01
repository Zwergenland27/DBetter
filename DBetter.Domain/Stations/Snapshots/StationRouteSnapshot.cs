using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations.Snapshots;

public record StationRouteSnapshot
{
    public required EvaNumber EvaNumber { get; init; }
    
    public required StationName Name { get; init; }
    
    public required StationInfoId? InfoId { get; init; }
}