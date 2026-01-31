using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Connection section
/// </summary>
public class VerbindungsAbschnitt : IHasMessage, IHasDemandInformation
{
    /// <inheritdoc/>
    public List<HimMeldung>? HimMeldungen { get; set; }
    
    /// <inheritdoc/>
    public required List<RisNotiz> RisNotizen { get; set; }
    
    /// <inheritdoc/>
    public required List<PriorisierteMeldung> PriorisierteMeldungen { get; set; }
    
    /// <summary>
    /// Id for information about the first station
    /// </summary>
    /// <example>1343</example>
    public string? ExterneBahnhofsinfoIdOrigin { get; set; }
    
    /// <summary>
    /// Id for information about the last station
    /// </summary>
    /// <example>1866</example>
    public string? ExterneBahnhofsinfoIdDestination { get; set; }
    
    /// <summary>
    /// Planned start time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// </remarks>
    /// <example>2025-03-15T19:07:00</example>
    public required string AbfahrtsZeitpunkt { get; set; }
    
    /// <summary>
    /// Real departure time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// Null, if no real time data is available
    /// </remarks>
    /// <example>2025-03-15T19:08:00</example>
    public string? EzAbfahrtsZeitpunkt { get; set; }
    
    /// <summary>
    /// Name of the first station
    /// </summary>
    /// <example>Dresden Hbf</example>
    public required string AbfahrtsOrt { get; set; }
    
    /// <summary>
    /// Eva number of the first station
    /// </summary>
    /// <example>8010085</example>
    public required string AbfahrtsOrtExtId { get; set; }
    
    /// <summary>
    /// Planned duration of the section in seconds
    /// </summary>
    /// <example>7140</example>
    public required int AbschnittsDauer { get; set; }
    
    /// <summary>
    /// Real duration of the section in seconds
    /// </summary>
    /// <remarks>
    /// Null, if no real time data is available
    /// </remarks>
    /// <example>7140</example>
    public int? EzAbschnittsDauerInSeconds { get; set; }
    
    /// <summary>
    /// Planned percentage of the connection section of the total connection duration
    /// </summary>
    /// <example>39.53</example>
    public required float AbschnittsAnteil { get; set; }
    
    /// <summary>
    /// Planned arrival time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// </remarks>
    /// <example>2025-03-15T19:07:00</example>
    public required string AnkunftsZeitpunkt { get; set; }
    
    /// <summary>
    /// Real arrival time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// Null, if no real time data is available
    /// </remarks>
    /// <example>2025-03-15T19:08:00</example>
    public string? EzAnkunftsZeitpunkt { get; set; }
    
    /// <summary>
    /// Name of the last station
    /// </summary>
    /// <example>Frankfurt(Main)Hbf</example>
    public required string AnkunftsOrt { get; set; }
    
    /// <summary>
    /// Eva number of the last station
    /// </summary>
    /// <example>8000105</example>
    public required string AnkunftsOrtExtId { get; set; }
    
    /// <inheritdoc/>
    public required List<AuslastungsMeldung> Auslastungsmeldungen { get; set; }
    
    /// <summary>
    /// List of all stops
    /// </summary>
    public required List<Halt> Halte { get; set; }
    
    /// <summary>
    /// Index of the section of the connection
    /// </summary>
    /// <example>0</example>
    public required int Idx { get; set; }
    
    /// <summary>
    /// Id of the full journey
    /// </summary>
    /// <example>2|#VN#1#ST#1741866428#PI#0#ZI#350936#TA#4#DA#150325#1S#8010085#1T#1653#LS#8010026#LT#1755#PU#80#RT#1#CA#DPN#ZE#52973#ZB#TL 52973#PC#3#FR#8010085#FT#1653#TO#8010026#TT#1755#</example>
    /// <remarks>
    /// Null, if section is of type walking
    /// </remarks>
    public string? JourneyId { get; set; }
    
    /// <summary>
    /// Information about transport method
    /// </summary>
    public required Verkehrsmittel Verkehrsmittel { get; set; }
    
    /// <summary>
    /// Walking Distance in m, if section is of type walking
    /// </summary>
    /// <example>116</example>
    public int? Distanz { get; set; }
    
    /// <summary>
    /// Indicator on how likely the next transport method is reached
    /// </summary> 5 = Anschluss wartet nicht; 1 - Anschluss wartet; 2 - Anschluss voraussichtlich erreichbar, 3 - Anschluss voraussichtlich nicht erreichbar? 4 - Anschluss nicht erreichbar
    public int? AnschlussBewertungCode { get; set; }

    public BahnJourneyId GetJourneyId(){
        if(JourneyId is null) throw new BahnDeException("Verbindungsabschnitt", "A walking section does not have a journeyId");
        return new BahnJourneyId(JourneyId);
    }

    public List<EvaNumber> GetEvaNumbers(){
        return Halte
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .ToList();
    }
}