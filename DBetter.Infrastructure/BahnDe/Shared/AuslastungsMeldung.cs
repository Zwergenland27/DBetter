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
    public required Klasse Klasse { get; set; }
    
    /// <summary>
    /// Demand
    /// </summary>
    public required AuslastungsStufe Stufe { get; set; }
}