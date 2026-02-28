using DBetter.Domain.TrainCompositions.ValueObjects;

namespace DBetter.Application.TrainCompositions.Dtos;

public class TrainCompositionResultDto
{
    public required string TrainRunId { get; init; }
    public required List<string> Vehicles { get; init; }
    
    public required TrainFormationSource Source { get; init; }
}