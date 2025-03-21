namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Message
/// </summary>
public class Meldung
{
    /// <summary>
    /// Code of the message
    /// </summary>
    /// <example>MDA-AK-MSG-1000</example>
    public string Code { get; set; }
    
    /// <summary>
    /// Short message
    /// </summary>
    /// <example>Connection is in the past.</example>
    public string NachrichtKurz { get; set; }
    
    /// <summary>
    /// Long message
    /// </summary>
    /// <example>Selected connection is in the past.</example>
    public string NachrichtLang { get; set; }
}