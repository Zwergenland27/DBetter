namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

/// <summary>
/// Reisenden Informations System message
/// </summary>
public class RisNotiz
{
    /// <summary>
    /// Key of the message
    /// </summary>
    /// <remarks>
    /// This can be a unique and useful id or a two-letter code that maps many different values
    /// </remarks>
    /// <example>text.realtime.connection.platform.change</example>
    public string Key { get; set; }
    
    /// <summary>
    /// Detailed message
    /// </summary>
    /// <example>TL 52973 departs differently from Dresden Hbf from Platform 13</example>
    public string Value { get; set; }
    
    /// <summary>
    /// Message is relevant from this route index until <see cref="RouteIdxTo"/>
    /// </summary>
    public int? RouteIdxFrom { get; set; }
    
    /// <summary>
    /// Message is relevant from <see cref="RouteIdxFrom"/> until this route index
    /// </summary>
    public int? RouteIdxTo { get; set; }
}