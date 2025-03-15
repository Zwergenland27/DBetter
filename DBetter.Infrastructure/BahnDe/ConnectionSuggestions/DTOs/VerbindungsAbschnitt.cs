namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

/// <summary>
/// Connection section
/// </summary>
public class VerbindungsAbschnitt
{
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
    /// Id for information about the first station
    /// </summary>
    /// <example>1343</example>
    public string ExterneBahnhofsinfoIdOrigin { get; set; }
    
    /// <summary>
    /// Id for information about the last station
    /// </summary>
    /// <example>1866</example>
    public string ExterneBahnhofsinfoIdDestination { get; set; }
    
    /// <summary>
    /// Planned start time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// </remarks>
    /// <example>2025-03-15T19:07:00</example>
    public string AbfahrtsZeitpunkt { get; set; }
    
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
    public string AbfahrtsOrt { get; set; }
    
    /// <summary>
    /// Eva number of the first station
    /// </summary>
    /// <example>8010085</example>
    public string AbfahrtsOrtExtId { get; set; }
    
    /// <summary>
    /// Planned duration of the section in seconds
    /// </summary>
    /// <example>7140</example>
    public int AbschnittsDauer { get; set; }
    
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
    public float AbschnittsAnteil { get; set; }
    
    /// <summary>
    /// Planned arrival time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// </remarks>
    /// <example>2025-03-15T19:07:00</example>
    public string AnkunftsZeitpunkt { get; set; }
    
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
    public string AnkunftsOrt { get; set; }
    
    /// <summary>
    /// Eva number of the last station
    /// </summary>
    /// <example>8000105</example>
    public string AnkunftsOrtExtId { get; set; }
    
    /// <summary>
    /// Demand
    /// </summary>
    public List<AuslastungsMeldung> Auslastungsmeldungen { get; set; }
    
    /// <summary>
    /// List of all stops
    /// </summary>
    public List<Halt> Halte { get; set; }
    
    /// <summary>
    /// Index of the section of the connection
    /// </summary>
    /// <example>0</example>
    public int Idx { get; set; }
    
    /// <summary>
    /// Id of the full journey
    /// </summary>
    /// <example>2|#VN#1#ST#1741866428#PI#0#ZI#350936#TA#4#DA#150325#1S#8010085#1T#1653#LS#8010026#LT#1755#PU#80#RT#1#CA#DPN#ZE#52973#ZB#TL 52973#PC#3#FR#8010085#FT#1653#TO#8010026#TT#1755#</example>
    public string JourneyId { get; set; }
    
    /// <summary>
    /// Information about transport method
    /// </summary>
    public Verkehrsmittel Verkehrsmittel { get; set; }
    
    /// <summary>
    /// Walking Distance in m, if section is of type walking
    /// </summary>
    /// <example>116</example>
    public int? Distanz { get; set; }
}