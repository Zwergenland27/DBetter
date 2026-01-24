using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Stations.Dtos;

public record StationProviderDto(
    Coordinates? Position,
    StationInfoId? InfoId,
    Ril100Identifier?  Ril100);