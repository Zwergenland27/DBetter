namespace DBetter.Domain.Routes.ValueObjects;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);