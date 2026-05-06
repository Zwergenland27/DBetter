using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

public class Fahrt : IHasMessage
{
    /// <summary>
    /// The day of the train run
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-dd german time zone
    /// </remarks>
    /// <example>2025-03-23</example>
    public string? ReiseTag { get; set; }
    
    /// <summary>
    /// Information about regularity of the train run
    /// </summary>
    /// <example>nicht täglich</example>
    public string? RegulaereVerkehrstage { get; set; }
    
    /// <summary>
    /// Information about irregular days of the train run
    /// </summary>
    /// <remarks>
    /// This string does not have a default format.
    /// </remarks>
    /// <example>23. Mär bis 9. Aug 2025 Sa, So; 5. bis 26. Apr 2025 So; nicht 20., 27. Apr; auch 29. Mai, 9. Jun bis 8. Aug 2025</example>
    public string? IrregulaereVerkehrstage { get; set; }
    
    /// <summary>
    /// Name of the train run
    /// </summary>
    /// <remarks>
    /// For some services this contains the correct line number, for others not
    /// </remarks>
    /// <example>RE 16518</example>
    public required string ZugName { get; set; }
    
    /// <inheritdoc/>
    public List<HimMeldung>? HimMeldungen { get; set; }
    
    /// <inheritdoc/>
    public required List<RisNotiz> RisNotizen { get; set; }
    
    /// <inheritdoc/>
    public required List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    /// <summary>
    /// List of all stops
    /// </summary>
    public required List<Halt> Halte { get; set; }
    
    /// <summary>
    /// Additional information about the train
    /// </summary>
    public required List<Zugattribut> Zugattribute { get; set; }
    
    /// <summary>
    /// Indicates wether the train run is completely cancelled or not
    /// </summary>
    public required bool Cancelled { get; set; }

    public List<EvaNumber> GetEvaNumbers(){
        return Halte
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .ToList();
    }
} 