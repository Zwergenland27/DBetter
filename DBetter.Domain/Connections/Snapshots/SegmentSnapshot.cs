namespace DBetter.Domain.Connections.Snapshots;

public record SegmentSnapshot
{
    public required int SegmentIndex { get; init; }
}