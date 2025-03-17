namespace DBetter.Domain.Connections.ValueObjects;

public record Platform(string Planned, string? Realtime, PlatformType Type);