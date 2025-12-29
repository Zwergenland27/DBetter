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
}