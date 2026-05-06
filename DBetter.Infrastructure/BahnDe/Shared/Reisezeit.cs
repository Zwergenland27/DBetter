namespace DBetter.Infrastructure.BahnDe.Shared;

/// <summary>
/// Departure or arrival time containing planned and real time
/// </summary>
public class Reisezeit
{
    /// <summary>
    /// Real departure / arrival time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// Null, if the train does not depart at the stop or no real time data is available
    /// </remarks>
    /// <example>2025-03-15T19:08:00</example>
    public string? Echtzeit { get; set; }
    
    /// <summary>
    /// Planned departure / arrival time
    /// </summary>
    /// <remarks>
    /// Time format: yyyy-mm-ddTHH:MM:ss german time zone
    /// Null, if the train does not depart at the stop
    /// </remarks>
    /// <example>2025-03-15T19:08:00</example>
    public required string Sollzeit { get; set; }
}