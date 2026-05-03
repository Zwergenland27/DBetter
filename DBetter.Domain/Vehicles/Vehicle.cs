using DBetter.Domain.Abstractions;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

public abstract class Vehicle: AggregateRoot<VehicleId>
{
    public EuropeanVehicleNumber Evn { get; private set; }
    
    internal Vehicle(VehicleId id, EuropeanVehicleNumber evn) : base(id)
    {
        Evn = evn;
    }
}