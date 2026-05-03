using DBetter.Domain.Abstractions;
using DBetter.Domain.MultipleUnitVehicles.Coaches.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.MultipleUnitVehicles.Coaches;

public class MultipleUnitCoach: Entity<MultipleUnitCoachId>
{
    public VehicleId Vehicle { get; private init; }
    
    internal MultipleUnitCoach(MultipleUnitCoachId id, VehicleId vehicle) : base(id)
    {
        Vehicle = vehicle;
    }
}