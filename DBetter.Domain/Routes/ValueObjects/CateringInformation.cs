namespace DBetter.Domain.Route.ValueObjects;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);