using DBetter.Domain.Shared;

namespace DBetter.Domain.TrainRun.ValueObjects;

public record Platform(string Planned, string? Realtime, PlatformType Type);