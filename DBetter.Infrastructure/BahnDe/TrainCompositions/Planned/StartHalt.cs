namespace DBetter.Infrastructure.BahnDe.TrainCompositions.Planned;

public class StartHalt
{
    public required string LocationId { get; init; }
    public required string AbfahrtZeit { get; init; }
}