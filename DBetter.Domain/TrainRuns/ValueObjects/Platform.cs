namespace DBetter.Domain.TrainRuns.ValueObjects;

public record Platform(string Planned, string? Real, PlatformType Type);