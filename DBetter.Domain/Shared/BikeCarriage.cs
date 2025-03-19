using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Shared;

public record BikeCarriage(
    BikeStatus Status,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);