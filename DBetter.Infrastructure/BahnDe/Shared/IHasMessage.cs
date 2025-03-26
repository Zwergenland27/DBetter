using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

public interface IHasMessage
{
    /// <summary>
    /// Messages from Hafas Information Manager
    /// </summary>
    public List<HimMeldung>? HimMeldungen { get; set; }
    
    /// <summary>
    /// Messages from Reisenden Informations System
    /// </summary>
    public List<RisNotiz> RisNotizen { get; set; }
    
    /// <summary>
    /// Prioritized messages
    /// </summary>
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
}