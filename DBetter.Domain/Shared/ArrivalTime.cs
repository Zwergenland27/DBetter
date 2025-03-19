namespace DBetter.Domain.Shared;

public record ArrivalTime(
    DateTime Planned,
    DateTime? Real);