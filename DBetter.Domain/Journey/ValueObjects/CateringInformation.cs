using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Journey.ValueObjects;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);