using DBetter.Domain.Shared;

namespace DBetter.Domain.TrainRun.ValueObjects;

public record BikeCarriage(
    BikeStatus Status,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);