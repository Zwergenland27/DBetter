namespace DBetter.Contracts.TrainCompositions.Get;

public class GetTrainCompositionResultDto
{
    public required List<string> Vehicles { get; set; }
    
    public required string Source { get; init; } 
}