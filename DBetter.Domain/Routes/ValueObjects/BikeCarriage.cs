namespace DBetter.Domain.Routes.ValueObjects;

public record BikeCarriageInformation(
    BikeCarriageStatus CarriageStatus,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);