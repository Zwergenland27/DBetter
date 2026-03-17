using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class RouteRepository(DBetterContext db) : IRouteRepository
{
    public void AddRange(IEnumerable<Route> routes)
    {
        db.AddRange(routes);
    }

    public void Add(Route route)
    {
        db.Add(route);
    }

    public Task<Route?> GetAsync(TrainRunId trainRunId)
    {
        return db.Routes.FirstOrDefaultAsync(r => r.TrainRunId == trainRunId);
    }

    public Task<Route?> GetAsync(RouteId routeId)
    {
        return db.Routes.FirstOrDefaultAsync(r => r.Id == routeId);
    }

    public Task<List<Route>> GetManyAsync(IEnumerable<TrainRunId> trainRunIds)
    {
        return db.Routes.Where(r => trainRunIds.Contains(r.TrainRunId))
            .ToListAsync();
    }
}