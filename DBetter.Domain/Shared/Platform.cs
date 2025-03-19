using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Shared;

public record Platform(string Planned, string? Realtime, PlatformType Type);