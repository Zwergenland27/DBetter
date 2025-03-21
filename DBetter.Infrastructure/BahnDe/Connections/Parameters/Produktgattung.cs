namespace DBetter.Infrastructure.BahnDe.Connections.Parameters;

/// <summary>
/// Allowed Vehicle type
/// </summary>
public enum Produktgattung
{
    /// <summary>
    /// ICE, RJ
    /// </summary>
    ICE,
    
    /// <summary>
    /// EC, IC,
    /// </summary>
    EC_IC,
    
    /// <summary>
    /// FLX, ES
    /// </summary>
    IR,
    
    /// <summary>
    /// Regional Trains (RE, RB, IRE)
    /// </summary>
    REGIONAL,
    
    /// <summary>
    /// S-Bahn
    /// </summary>
    SBAHN,
    
    /// <summary>
    /// Bus
    /// </summary>
    BUS,
    
    /// <summary>
    /// Boat, Ferry
    /// </summary>
    SCHIFF,
    
    /// <summary>
    /// Underground
    /// </summary>
    UBAHN,
    
    /// <summary>
    /// Tram
    /// </summary>
    TRAM,
    
    /// <summary>
    /// Services requiring telephonic registration
    /// </summary>
    ANRUFPFLICHTIG
}