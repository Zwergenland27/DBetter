namespace DBetter.Domain.TrainRun.ValueObjects;

public record DepartureTime(
    DateTime Planned,
    DateTime? Real);