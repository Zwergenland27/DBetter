namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Classes that implement this contain demand information
/// </summary>
public interface IHasDemandInformation
{
    /// <summary>
    /// Demand information
    /// </summary>
    public List<AuslastungsMeldung> Auslastungsmeldungen { get; set; }
}