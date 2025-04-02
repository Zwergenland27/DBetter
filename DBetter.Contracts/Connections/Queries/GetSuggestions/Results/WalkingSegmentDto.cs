namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Longer walking segment
/// </summary>
public class WalkingSegmentDto : SegmentDto
{
    /// <summary>
    /// Estimated walking distance in meters
    /// </summary>
    public required int Distance { get; set; }
    
    /// <summary>
    /// Estimated walking duration in minutes
    /// </summary>
    public required int Duration { get; set; }
}