namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.Parameters;

/// <summary>
/// Type of passenger group
/// </summary>
public enum ReisenderTyp
{
    /// <summary>
    /// Bike
    /// </summary>
    FAHRRAD,
    /// <summary>
    /// Dog
    /// </summary>
    HUND,
    /// <summary>
    /// Child (age less or equal 5)
    /// </summary>
    KLEINKIND,
    /// <summary>
    /// child (age less or equal 14)
    /// </summary>
    FAMILIENKIND,
    /// <summary>
    /// teenager (age less or equal 26)
    /// </summary>
    JUGENDLICHER,
    /// <summary>
    /// adult (age less or equal 64)
    /// </summary>
    ERWACHSENER,
    /// <summary>
    /// senior (age larger than 64)
    /// </summary>
    SENIOR,
}