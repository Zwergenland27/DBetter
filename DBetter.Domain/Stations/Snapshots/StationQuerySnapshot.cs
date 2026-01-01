using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations.Snapshots;

public record StationQuerySnapshot(
    EvaNumber EvaNumber,
    StationName Name,
    Coordinates Location);