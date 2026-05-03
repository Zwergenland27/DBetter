using DBetter.Domain.MultipleUnitVehicles.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainCompositions.TrainParts;

public class MultipleUnitVehicleTrainPart: ResolvedTrainPart
{
    public MultipleUnitVehicleId VehicleId { get; private set; }
    
    public MultipleUnitVehicleTrainPart(MultipleUnitVehicleTrainPart original) : base(original)
    {
        VehicleId = original.VehicleId;
    }

    public MultipleUnitVehicleTrainPart(StationId fromStation, StationId toStation, MultipleUnitVehicleId vehicleId1) : base(fromStation, toStation)
    {
        VehicleId = vehicleId1;
    }
}