using System.ComponentModel.DataAnnotations;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

/// <summary>
/// Stop of a train run
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
    
    /// <inheritdoc/>
    public string? EzGleis { get; set; }
    
    /// <inheritdoc/>
    public required HaltTyp HaltTyp { get; set; }
    
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
    
    /// <summary>
    /// Contains information about the service type.
    /// This value is (when set) always <see cref="Verkehrsmittel.KurzText"/> and thus not really useful
    /// </summary>
    /// <example>TLX</example>
    public string? Kategorie { get; set; }
    
    /// <summary>
    /// Number of the train run
    /// </summary>
    /// <remarks>
    /// Contains (sometimes) train number for most services. Useful for trains (like suburbans or trams) that do not
    /// contain a tain number on initial request
    /// </remarks>
    /// <example>16518</example>
    public string? Nummer { get; set; }
}