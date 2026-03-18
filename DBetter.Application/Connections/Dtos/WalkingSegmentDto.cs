namespace DBetter.Application.Connections.Dtos;

public record WalkingSegmentDto : TransferSegmentDto
{
    public required int Distance { get; init; }
    
    public required int WalkDuration { get; init; }
}