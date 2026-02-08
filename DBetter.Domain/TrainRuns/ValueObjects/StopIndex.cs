namespace DBetter.Domain.TrainRuns.ValueObjects;

[Obsolete("Has issues with dynamic route changes like additional stops and thus is not recommended to use anymore. Use station ids instead and use route to get the real index dynamically")]
public record StopIndex(int Value);