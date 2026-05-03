using DBetter.Domain.Abstractions;
using DBetter.Domain.Consists.Coaches.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Consists.Coaches;

public class ConsistCoach: Entity<ConsistCoachId>
{
    public VehicleId Vehicle { get; private init; }
    
    internal ConsistCoach(ConsistCoachId id, VehicleId vehicle) : base(id)
    {
        Vehicle = vehicle;
    }
}