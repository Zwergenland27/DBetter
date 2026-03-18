using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes.Snapshots;

public record StopSnapshot(
    StopIndex RouteIndex,
    StationId StationId,
    TravelTime? ArrivalTime,
    TravelTime? DepartureTime,
    Demand Demand,
    Platform? Platform,
    StopAttributes Attributes);
