namespace DBetter.Domain.TrainRun.ValueObjects;

public record ArrivalTime(
    DateTime Planned,
    DateTime? Real);