namespace DBetter.Domain.Routes.ValueObjects;

public record Platform(string Planned, string? Real, PlatformType Type);