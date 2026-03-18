namespace DBetter.Application.TrainCompositions.Dtos;

public class PlannedTrainCompositionDto
{
    public required List<PlannedVehicleDto> Vehicles { get; init; }
}