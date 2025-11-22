namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Stopover
/// </summary>
public class Zwischenhalt
{
    /// <summary>
    /// Duration of stay in minutes
    /// </summary>
    /// <remarks>
    /// When the duration is zero, this field must be null or an error is thrown
    /// </remarks>
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