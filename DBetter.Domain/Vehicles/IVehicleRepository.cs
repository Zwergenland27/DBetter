using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

public interface IVehicleRepository
{
    void Add(Vehicle vehicle);
    
    Task<Vehicle?> GetAsync(VehicleId vehicleId);
    
    Task<List<Vehicle>> GetManyAsync(IEnumerable<VehicleId> vehicleIds);

    Task<Vehicle?> FindByConstructionTypeAsync(List<string> coachSequence);

    Task<Vehicle?> FindByCoachType(List<string> coachSequence);
}