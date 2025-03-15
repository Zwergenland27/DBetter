using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Parameters;

namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

/// <summary>
/// Response to <see cref="ReiseAnfrage"/>
/// </summary>
public class Fahrplan
{
    /// <summary>
    /// Suggested connections
    /// </summary>
    public List<Verbindung> Verbindungen { get; set; }
    
    /// <summary>
    /// Pagination references
    /// </summary>
    public VerbindungsReference VerbindungReference { get; set; }
}