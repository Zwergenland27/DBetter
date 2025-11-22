namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Information about the means of transport of a section
/// </summary>
public enum VerkehrsmittelTyp
{
    /// <summary>
    /// Walking section
    /// </summary>
    WALK,
    
    /// <summary>
    /// Public transport section
    /// </summary>
    PUBLICTRANSPORT,
    
    /// <summary>
    /// Longer walking section?
    /// </summary>
    TRANSFER
}