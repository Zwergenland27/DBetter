using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// A connection result
/// </summary>
public class Verbindung : IHasMessage, IHasDemandInformation
{
    /// <summary>
    /// Id of the connection
    /// </summary>
    /// <example>59e9c36c_3</example>
    public required string TripId { get; set; }
    
    /// <summary>
    /// Some other id
    /// </summary>
    /// <remarks>
    /// This id is needed for bahn.de url generation
    /// </remarks>
    public required string CtxRecon { get; set; }
    
    /// <summary>
    /// Connection sections
    /// </summary>
    public required List<VerbindungsAbschnitt> VerbindungsAbschnitte { get; set; }
    
    /// <summary>
    /// Number of transfers
    /// </summary>
    public required int UmstiegsAnzahl { get; set; }
    
    /// <summary>
    /// Planned duration of the connection in seconds
    /// </summary>
    public required int VerbindungsDauerInSeconds { get; set; }
    
    /// <summary>
    /// Real duration of the conenction in seconds
    /// </summary>
    public int? EzVerbindungsDauerInSeconds { get; set; }
    
    /// <summary>
    /// Indicates wether the connection is an alternative to a cancelled connection
    /// </summary>
    public required bool IsAlternativeVerbindung { get; set; }
    
    /// <inheritdoc/>
    public required List<AuslastungsMeldung> Auslastungsmeldungen { get; set; }
    
    /// <summary>
    /// Information about bike carriage
    /// </summary>
    public Fahrradmitnahme? FahrradmitnahmeMoeglich { get; set; }
    
    /// <inheritdoc/>
    public List<HimMeldung>? HimMeldungen { get; set; }
    
    /// <inheritdoc/>
    public required List<RisNotiz> RisNotizen { get; set; }
    
    /// <inheritdoc/>
    public required List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    /// <summary>
    /// True, if the <see cref="Reisender.Alter"/> is required
    /// </summary>
    public bool? IsAlterseingabeErfoderlich { get; set; }
    
    /// <summary>
    /// List of known service days
    /// </summary>
    public required List<ServiceDay> ServiceDays { get; set; }
    
    /// <summary>
    /// Price offer
    /// </summary>
    public AngebotsPreis? AngebotsPreis { get; set; }
    
    /// <summary>
    /// Price category of the price offer
    /// </summary>
    /// <see cref="Klasse"/>
    public string? AngebotsPreisKlasse { get; set; }
    
    /// <summary>
    /// Indicates wether the price is only for a part of the route
    /// </summary>
    public required bool HasTeilpreis {get; set; }
    
    /// <summary>
    /// Messages
    /// </summary>
    public List<Meldung>? MeldungenAsObject { get; set; }

    public List<BahnJourneyId> GetJourneyIds(){
        return VerbindungsAbschnitte
            .Where(va => va.JourneyId is not null)
            .Select(va => va.GetJourneyId())
            .ToList();
    }

    public List<EvaNumber> GetEvaNumbers(){
        return VerbindungsAbschnitte
            .SelectMany(va => va.GetEvaNumbers())
            .ToList();
    }
}