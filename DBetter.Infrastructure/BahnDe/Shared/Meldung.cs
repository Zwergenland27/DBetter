namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Message
/// </summary>
public class Meldung
{
    /// <summary>
    /// Code of the message
    /// </summary>
    /// <example>MDA-AK-MSG-1000</example>
    public required string Code { get; set; }
    
    /// <summary>
    /// Short message
    /// </summary>
    /// <example>Connection is in the past.</example>
    public required string NachrichtKurz { get; set; }
    
    /// <summary>
    /// Long message
    /// </summary>
    /// <example>Selected connection is in the past.</example>
    public required string NachrichtLang { get; set; }
}