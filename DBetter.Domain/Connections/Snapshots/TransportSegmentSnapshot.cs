using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Connections.Snapshots;

public record TransportSegmentSnapshot : SegmentSnapshot
{
    public required StationId FirstStationId { get; init; }
    
    public required DateTime PlannedDepartureTime { get; init; }
    
    public required StationId LastStationId { get; init; }
    
    public required DateTime PlannedArrivalTime { get; init; }
}