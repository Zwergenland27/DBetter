using DBetter.Domain.Abstractions;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

//TODO: Rename to TrainPart
public class FormationVehicle : Entity<FormationVehicleId>
{
    public VehicleId VehicleId { get; private set; }
    
    public StationId FromStation { get; private set; } 
    
    public StationId ToStation { get; private set; }
    
    private FormationVehicle() : base(null!)
    {
    }

    internal FormationVehicle(
        FormationVehicleId id,
        VehicleId vehicleId,
        StationId fromStation,
        StationId toStation) : base(id)
    {
        VehicleId = vehicleId;
        FromStation = fromStation;
        ToStation = toStation;
    }
}