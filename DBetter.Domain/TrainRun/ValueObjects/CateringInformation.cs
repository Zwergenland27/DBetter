using DBetter.Domain.Shared;

namespace DBetter.Domain.TrainRun.ValueObjects;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);