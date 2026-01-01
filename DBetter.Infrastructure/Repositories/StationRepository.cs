using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class StationRepository(DBetterContext db) : IStationRepository
{
    public Task<Station?> GetAsync(StationId id)
    {
        return db.Stations.FirstOrDefaultAsync(station => station.Id == id);
    }

    public Task<Station?> FindByRil100Async(Ril100Identifier ril100Identifier)
    {
        return db.Stations.FirstOrDefaultAsync(station => station.Ril100 == ril100Identifier);
    }

    public Task<List<Station>> GetManyAsync(IEnumerable<EvaNumber> evaNumbers)
    {
        return db.Stations.Where(station => evaNumbers.Contains(station.EvaNumber)).ToListAsync();
    }

    public void AddRange(IEnumerable<Station> stations)
    {
        db.Stations.AddRange(stations);
    }
}