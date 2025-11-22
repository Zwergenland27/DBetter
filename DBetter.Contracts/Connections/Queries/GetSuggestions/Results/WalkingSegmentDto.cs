namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Transfer segment with longer walking distance
/// </summary>
public class WalkingSegmentDto : TransferSegmentDto
{
    /// <summary>
    /// Estimated walking distance in meters
    /// </summary>
    public required int Distance { get; set; }
    
    /// <summary>
    /// Estimated walking duration in minutes
    /// </summary>
    public required int WalkDuration { get; set; }
}