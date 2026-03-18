using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Stop of a train run that is part of a connection
/// </summary>
public class Halt : IHasMessage, ITrainRunStop, IHasDemandInformation
{
    /// <inheritdoc/>
    public required string Id { get; set; }
    
    /// <inheritdoc/>
    public string? AbfahrtsZeitpunkt { get; set; }
    
    /// <inheritdoc/>
    public string? EzAbfahrtsZeitpunkt { get; set; }
    
    /// <inheritdoc/>
    public string? AnkunftsZeitpunkt { get; set; }
    
    /// <inheritdoc/>
    public string? EzAnkunftsZeitpunkt { get; set; }
    
    /// <inheritdoc/>
    public required List<AuslastungsMeldung> Auslastungsmeldungen { get; set; }
    
    /// <inheritdoc/>
    public string? Gleis { get; set; }
    
    //// <inheritdoc/>
    public string? EzGleis { get; set; }
    
    /// <inheritdoc/>
    public HaltTyp? HaltTyp { get; set; }
    
    /// <inheritdoc/>
    public required string Name { get; set; }
    
    /// <inheritdoc/>
    public List<HimMeldung>? HimMeldungen { get; set; }
    
    /// <inheritdoc/>
    public required List<RisNotiz> RisNotizen { get; set; }
    
    /// <inheritdoc/>
    public required List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    /// <inheritdoc/>
    public string? BahnhofsInfoId { get; set; }
    
    /// <inheritdoc/>
    public required string ExtId { get; set; }
    
    /// <inheritdoc/>
    public required int RouteIdx { get; set; }
}