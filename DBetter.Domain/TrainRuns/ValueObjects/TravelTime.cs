namespace DBetter.Domain.TrainRuns.ValueObjects;

public record TravelTime(
    DateTime Planned,
    DateTime? Real);