namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Parameters;

/// <summary>
/// Stopover
/// </summary>
public class Zwischenhalt
{
    /// <summary>
    /// Duration of stay in minutes
    /// </summary>
    public required int? Aufenthaltsdauer { get; set; }
    
    /// <summary>
    /// Fuzzy station id
    /// </summary>
    /// <example>@L=8010085</example>
    public required string Id { get; set; }
    
    /// <summary>
    /// Allowed vehicles for the next section
    /// </summary>
    public required List<Produktgattung> VerkehrsmittelOfNextAbschnitt  { get; set; }
}