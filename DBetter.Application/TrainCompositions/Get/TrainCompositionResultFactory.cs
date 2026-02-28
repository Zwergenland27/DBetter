using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.Vehicles;

namespace DBetter.Application.TrainCompositions.Get;

public class TrainCompositionResultFactory(
    TrainComposition trainComposition,
    List<Vehicle> relevantVehicles)
{
    public GetTrainCompositionResultDto BuildResult()
    {
        return new GetTrainCompositionResultDto
        {
            Vehicles = trainComposition.Vehicles
                .Select(v => relevantVehicles.First(rv => rv.Id == v.VehicleId).Identifier)
                .ToList(),
            Source = trainComposition.Source.ToString()
        };
    }
}