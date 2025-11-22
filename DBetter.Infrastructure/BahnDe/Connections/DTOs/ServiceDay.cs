namespace DBetter.Infrastructure.BahnDe.Connections.DTOs;

/// <summary>
/// Information about service days of a connection
/// </summary>
public class ServiceDay
{
    /// <summary>
    /// Last date of current repeating period (for example weekly or monthly)
    /// </summary>
    /// <remarks>
    /// Date in format yyyy-mm-dd german time zone
    /// </remarks>
    /// <example>2025-03-16</example>
    public required string LastDateInPeriod { get; set; }
    
    /// <summary>
    /// Begin of the planning period
    /// </summary>
    /// <remarks>
    /// Date in format yyyy-mm-dd german time zone
    /// </remarks>
    /// <example>2024-12-15</example>
    public required string PlanningPeriodBegin { get; set; }
    
    /// <summary>
    /// End of the currently planned period
    /// </summary>
    /// <remarks>
    /// Date in format yyyy-mm-dd german time zone
    /// </remarks>
    /// <example>2025-12-13</example>
    public required string PlanningPeriodEnd { get; set; }
    
    /// <summary>
    /// List of the week days on which the train service will be present
    /// </summary>
    public required List<Weekday> Weekdays { get; set; }
}