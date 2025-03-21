using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// A connection result
/// </summary>
public class Verbindung : IHasMessage
{
    /// <summary>
    /// Id of the connection
    /// </summary>
    /// <example>59e9c36c_3</example>
    public string TripId { get; set; }
    
    /// <summary>
    /// Some other id
    /// </summary>
    /// <remarks>
    /// This id is needed for bahn.de url generation
    /// </remarks>
    public string CtxRecon { get; set; }
    
    /// <summary>
    /// Connection sections
    /// </summary>
    public List<VerbindungsAbschnitt> VerbindungsAbschnitte { get; set; }
    
    /// <summary>
    /// Number of transfers
    /// </summary>
    public int UmstiegsAnzahl { get; set; }
    
    /// <summary>
    /// Planned duration of the connection in seconds
    /// </summary>
    public int VerbindungsDauerInSeconds { get; set; }
    
    /// <summary>
    /// Real duration of the conenction in seconds
    /// </summary>
    public int? EzVerbindungsDauerInSeconds { get; set; }
    
    /// <summary>
    /// Indicates wether the connection is an alternative to a cancelled connection
    /// </summary>
    public bool IsAlternativeVerbindung { get; set; }
    
    /// <summary>
    /// Demand
    /// </summary>
    public List<AuslastungsMeldung> Auslastungsmeldungen { get; set; }
    
    /// <summary>
    /// Information about bike carriage
    /// </summary>
    public Fahrradmitnahme? FahrradmitnahmeMoeglich { get; set; }
    
    /// <summary>
    /// Messages from Hafas Information Manager
    /// </summary>
    public List<HimMeldung> HimMeldungen { get; set; }
    
    /// <summary>
    /// Messages from Reisenden Informations System
    /// </summary>
    public List<RisNotiz> RisNotizen { get; set; }
    
    /// <summary>
    /// Prioritized messages
    /// </summary>
    public List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    /// <summary>
    /// True, if the <see cref="Reisender.Alter"/> is required
    /// </summary>
    public bool IsAlterseingabeErfoderlich { get; set; }
    
    /// <summary>
    /// List of known service days
    /// </summary>
    public List<ServiceDay> ServiceDays { get; set; }
    
    /// <summary>
    /// Price offer
    /// </summary>
    public AngebotsPreis? AngebotsPreis { get; set; }
    
    /// <summary>
    /// Price category of the price offer
    /// </summary>
    public Klasse? AngebotsPreisKlasse { get; set; }
    
    /// <summary>
    /// Indicates wether the price is only for a part of the route
    /// </summary>
    public bool HasTeilpreis {get; set; }
    
    /// <summary>
    /// Unknown use case
    /// </summary>
    public bool HinRueckPauschalPreis { get; set; }
    
    /// <summary>
    /// Indicate wether the reservation is in pre-sale period
    /// </summary>
    public bool IsReservierungAusserhalbVorverkaufszeitraum { get; set; }
    
    /// <summary>
    /// Messages
    /// </summary>
    public List<Meldung> MeldungenAsObject { get; set; }
}