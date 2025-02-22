using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Stations;

public record Station(EvaNumber EvaNumber, StationName Name, Coordinates Position);