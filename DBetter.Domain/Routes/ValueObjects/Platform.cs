namespace DBetter.Domain.Route.ValueObjects;

public record Platform(string Planned, string? Realtime, PlatformType Type);