namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

/// <summary>
/// Additional information about a train
/// </summary>
public class Zugattribut
{
    /// <summary>
    /// Additional information type
    /// </summary>
    /// <example>BEFÖRDERER</example>
    public string Kategorie { get; set; }
    
    /// <summary>
    /// Code of information type
    /// </summary>
    /// <example>BEF</example>
    public string Key { get; set; }
    
    /// <summary>
    /// The information itself
    /// </summary>
    /// <example>trilex - Die Länderbahn GmbH DLB</example>
    public string Value { get; set; }
    
    /// <summary>
    /// Contains information about the section where the attribute is valid
    /// </summary>
    /// <remarks>
    /// This always contains only stations that are stops in the section.
    /// </remarks>
    /// <example>(Hannover Messe/Laatzen - Bennemühlen)</example>
    public string? Teilstreckenhinweis { get; set; }
}