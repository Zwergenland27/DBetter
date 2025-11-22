using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Occupancy information
/// </summary>
public class AuslastungsMeldung
{
    /// <summary>
    /// Comfort Class of the occupancy value
    /// </summary>
    /// <see cref="Klasse"/>
    public required string Klasse { get; set; }
    
    /// <summary>
    /// Demand
    /// </summary>
    public required AuslastungsStufe Stufe { get; set; }
}