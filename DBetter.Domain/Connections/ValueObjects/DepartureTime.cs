namespace DBetter.Domain.Connections.ValueObjects;

public record DepartureTime(
    DateTime Planned,
    DateTime? Real);