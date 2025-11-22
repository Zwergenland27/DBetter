using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Hafas Information Manager message
/// </summary>
public class HimMeldung
{
    /// <summary>
    /// Priority of the message
    /// </summary>
    public Prioritaet? Priority { get; set; }
    
    /// <summary>
    /// Title
    /// </summary>
    public required string Ueberschrift { get; set; }
    
    /// <summary>
    /// Text
    /// </summary>
    public string? Text { get; set; }
    
    /// <summary>
    /// Time of last information update
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// </remarks>
    /// <example>2025-03-15T19:08:00</example>
    public required string ModDateTime { get; set; }
}