using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations.Snapshots;

public record StationRouteSnapshot(
    EvaNumber EvaNumber,
    StationName Name,
    StationInfoId? InfoId);