using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public record StationInformation(
    Coordinates? Position,
    StationInfoId? InfoId,
    Ril100?  Ril100);