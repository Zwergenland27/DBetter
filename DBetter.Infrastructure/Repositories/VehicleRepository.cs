using DBetter.Domain.Vehicles;
using DBetter.Domain.Vehicles.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class VehicleRepository(DBetterContext db) : IVehicleRepository
{
    public void Add(Vehicle vehicle)
    {
        db.Add(vehicle);
    }

    public Task<Vehicle?> GetAsync(VehicleId vehicleId)
    {
        return db.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);
    }

    public async Task<Vehicle?> FindByConstructionTypeAsync(List<string> coachSequence)
    {
        var existingVehicles = await db.Vehicles.ToListAsync();
        return existingVehicles.FirstOrDefault(v => v.MatchesConstructionType(coachSequence));
    }

    public async Task<Vehicle?> FindByCoachType(List<string> coachSequence)
    {
        var existingVehicles = await db.Vehicles.ToListAsync();
        return existingVehicles.FirstOrDefault(v => v.MatchesCoachType(coachSequence));
    }
}