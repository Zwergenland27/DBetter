namespace DBetter.Application.Requests.Snapshots;

public record WalkingSegmentSnapshot : SegmentSnapshot
{
    public required int Distance { get; init; }
    
    public required int WalkDuration { get; init; }
}