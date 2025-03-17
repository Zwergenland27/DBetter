namespace DBetter.Domain.Connections.ValueObjects;

public record ArrivalTime(
    DateTime Planned,
    DateTime? Real);