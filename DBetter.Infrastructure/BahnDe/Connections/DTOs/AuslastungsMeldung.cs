using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Occupancy information
/// </summary>
public class AuslastungsMeldung
{
    /// <summary>
    /// Comfort Class of the occupancy value
    /// </summary>
    public Klasse Klasse { get; set; }
    
    /// <summary>
    /// Demand
    /// </summary>
    public AuslastungsStufe Stufe { get; set; }
}