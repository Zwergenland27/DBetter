using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Domain.Shared;

public record CateringInformation(
    CateringType Type,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);