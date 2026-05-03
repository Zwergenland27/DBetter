using DBetter.Domain.MultipleUnitVehicleSeriesVariant.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

public class Locomotive: Vehicle
{
    public PowerType PowerType { get; set; }
    
    public Locomotive(
        VehicleId id,
        EuropeanVehicleNumber evn,
        PowerType powerType) : base(id, evn)
    {
        PowerType = powerType;
    }
}