namespace DBetter.Domain.Shared;

public record DepartureTime(
    DateTime Planned,
    DateTime? Real);