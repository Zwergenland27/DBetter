namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Indicates wether the request time should be interpreted as departure or arrival time
/// </summary>
public enum AnkunftSuche
{
    /// <summary>
    /// Departure
    /// </summary>
    ABFAHRT,
    
    /// <summary>
    /// Arrival
    /// </summary>
    ANKUNFT
}