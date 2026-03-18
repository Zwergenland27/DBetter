namespace DBetter.Infrastructure.BahnDe.TrainCompositions.Planned;

public class EndHalt
{
    public required string LocationId { get; init; }
    public required string AnkunftZeit { get; init; }
}