using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.TrainCompositions.Dtos;

public record VehicleDto
{
    public required StationName DestinationStation { get; init; }
    public required string Name { get; init; }
    public required List<CoachDto> Coaches { get; init; }
}