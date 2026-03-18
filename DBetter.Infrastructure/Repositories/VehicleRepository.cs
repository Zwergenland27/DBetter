using DBetter.Application.Abstractions.Caching;
using DBetter.Domain.Vehicles;
using DBetter.Domain.Vehicles.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DBetter.Infrastructure.Repositories;

public class VehicleRepository(
    DBetterContext db,
    ICache cache) : IVehicleRepository
{
    public void Add(Vehicle vehicle)
    {
        db.Add(vehicle);
    }

    public async Task<Vehicle?> GetAsync(VehicleId vehicleId)
    {
        var cacheKey = $"vehicle:id:{vehicleId.Value.ToString()}";
        if (cache.TryGetValue<Vehicle>(cacheKey, out var cached))
        {
            return cached;
        }
        
        var vehicle = await db.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId);

        if (vehicle is not null)
        {
            cache.Set(cacheKey, vehicle);
        }
        
        return vehicle;
    }

    public async Task<List<Vehicle>> GetManyAsync(IEnumerable<VehicleId> vehicleIds)
    {
        var enumerable = vehicleIds.ToList();
        var cacheKey = $"vehicle:many:{string.Join("", enumerable.Select(v => v.Value.ToString()))}";

        if (cache.TryGetValue<List<Vehicle>>(cacheKey, out var cached))
        {
            return cached!;
        }
        
        var vehicles = await db.Vehicles.Where(v => enumerable.Contains(v.Id)).ToListAsync();

        cache.Set(cacheKey, vehicles);
        
        return vehicles;
    }

    public async Task<Vehicle?> FindByConstructionTypeAsync(List<string> coachSequence)
    {
        var cacheKey = $"vehicle:byconstruction:{string.Join("", coachSequence)}";

        if (cache.TryGetValue<Vehicle>(cacheKey, out var cached))
        {
            return cached;
        }
        
        var existingVehicles = await db.Vehicles.ToListAsync();
        var vehicle = existingVehicles.FirstOrDefault(v => v.MatchesConstructionType(coachSequence));
        if (vehicle is not null)
        {
            cache.Set(cacheKey, vehicle);
        }
        return vehicle;
    }

    public async Task<Vehicle?> FindByCoachType(List<string> coachSequence)
    {
        var cacheKey = $"vehicle:bycoach:{string.Join("", coachSequence)}";

        if (cache.TryGetValue<Vehicle>(cacheKey, out var cached))
        {
            return cached;
        }
        
        var existingVehicles = await db.Vehicles.ToListAsync();
        var vehicle = existingVehicles.FirstOrDefault(v => v.MatchesCoachType(coachSequence));
        if (vehicle is not null)
        {
            cache.Set(cacheKey, vehicle);
        }
        return vehicle;
    }
}