namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Demand
/// </summary>
public enum AuslastungsStufe
{
    Unknown = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Extreme = 4,
    Overbooked = 99
}