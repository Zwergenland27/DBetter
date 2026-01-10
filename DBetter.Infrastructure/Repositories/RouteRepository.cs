using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class RouteRepository(DBetterContext db) : IRouteRepository
{
    public Task<Route?> GetAsync(RouteId id)
    {
        return db.Routes.FirstOrDefaultAsync(r => r.Id == id);
    }

    public Task<List<Route>> GetManyAsync(IEnumerable<BahnJourneyId> journeyIds)
    {
        return db.Routes.Where(r => journeyIds.Contains(r.JourneyId)).ToListAsync();
    }

    public void AddRange(IEnumerable<Route> routes)
    {
        db.Routes.AddRange(routes);
    }
}