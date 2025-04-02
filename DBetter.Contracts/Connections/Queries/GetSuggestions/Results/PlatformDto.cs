namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Platform at a stop
/// </summary>
public class PlatformDto
{
    /// <summary>
    /// Planned platform
    /// </summary>
    public required string Planned { get; set; }
    
    /// <summary>
    /// Real platform
    /// </summary>
    public required string? Real { get; set; }
    
    /// <summary>
    /// Type of the platform
    /// </summary>
    /// <remarks>
    /// Not used at the moment
    /// </remarks>
    public required string Type { get; set; }
}