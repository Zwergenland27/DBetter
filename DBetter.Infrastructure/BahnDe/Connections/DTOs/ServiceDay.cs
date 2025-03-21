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
    public string LastDateInPeriod { get; set; }
    
    /// <summary>
    /// Begin of the planning period
    /// </summary>
    /// <remarks>
    /// Date in format yyyy-mm-dd german time zone
    /// </remarks>
    /// <example>2024-12-15</example>
    public string PlanningPeriodBegin { get; set; }
    
    /// <summary>
    /// End of the currently planned period
    /// </summary>
    /// <remarks>
    /// Date in format yyyy-mm-dd german time zone
    /// </remarks>
    /// <example>2025-12-13</example>
    public string PlanningPeriodEnd { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public List<Weekday> Weekdays { get; set; }
}