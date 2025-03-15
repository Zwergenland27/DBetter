using System.Text.Json.Serialization;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Parameters;

namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

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