namespace DBetter.Domain.Route.ValueObjects;

public record BikeCarriage(
    BikeCarriageStatus CarriageStatus,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);