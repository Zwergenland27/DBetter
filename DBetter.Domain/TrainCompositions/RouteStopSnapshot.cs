using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public record RouteStopSnapshot(int RouteIndex, StationId StationId);