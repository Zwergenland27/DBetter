namespace DBetter.Application.TrainCompositions.Dtos;

public record TrainCompositionDto
{
    public required List<VehicleDto> Vehicles { get; init; } 
};