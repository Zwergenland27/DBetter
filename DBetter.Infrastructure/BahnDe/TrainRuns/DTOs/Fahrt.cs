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
    public string ReiseTag { get; set; }
    
    /// <summary>
    /// Information about regularity of the train run
    /// </summary>
    /// <example>nicht täglich</example>
    public string RegulaereVerkehrstage { get; set; }
    
    /// <summary>
    /// Information about irregular days of the train run
    /// </summary>
    /// <remarks>
    /// This string does not have a default format.
    /// </remarks>
    /// <example>23. Mär bis 9. Aug 2025 Sa, So; 5. bis 26. Apr 2025 So; nicht 20., 27. Apr; auch 29. Mai, 9. Jun bis 8. Aug 2025</example>
    public string IrregulaereVerkehrstage { get; set; }
    
    /// <summary>
    /// Name of the train run
    /// </summary>
    /// <remarks>
    /// For some services this contains the correct line number, for others not
    /// </remarks>
    /// <example>RE 16518</example>
    public string ZugName { get; set; }
    
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
    /// List of all stops
    /// </summary>
    public List<Halt> Halte { get; set; }
    
    /// <summary>
    /// Additional information about the train
    /// </summary>
    public List<Zugattribut> Zugattribute { get; set; }
    
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
    /// Indicates wether the train run is completely cancelled or not
    /// </summary>
    public bool Cancelled { get; set; }
} 