using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Journey.ValueObjects;

public record BikeInformation(
    BikeTransport Status,
    StopIndex FromStopIndex,
    StopIndex ToStopIndex);