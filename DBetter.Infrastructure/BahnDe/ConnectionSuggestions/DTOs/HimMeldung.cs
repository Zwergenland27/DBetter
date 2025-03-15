namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

/// <summary>
/// Hafas Information Manager message
/// </summary>
public class HimMeldung
{
    /// <summary>
    /// Title
    /// </summary>
    public string Ueberschrift { get; set; }
    
    /// <summary>
    /// Text
    /// </summary>
    public string? Text { get; set; }
}