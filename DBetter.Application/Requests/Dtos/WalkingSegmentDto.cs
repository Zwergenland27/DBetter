namespace DBetter.Application.Requests.Dtos;

public record WalkingSegmentDto : SegmentDto
{
    public required int Distance { get; init; }
    
    public required int WalkDuration { get; init; }
}