using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Response to <see cref="ReiseAnfrage"/>
/// </summary>
public class Fahrplan
{
    /// <summary>
    /// Suggested connections
    /// </summary>
    public required List<Verbindung> Verbindungen { get; set; }
    
    /// <summary>
    /// Pagination references
    /// </summary>
    public required VerbindungsReference VerbindungReference { get; set; }
}