using DBetter.Application.Routes;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class RouteQueryRepository(
    DBetterContext context,
    RouteService service) : IRouteQueryRepository
{
    public async Task<RouteDto?> GetAsync(RouteId id)
    {
        var routeInfos = await context.Routes
            .FirstOrDefaultAsync(tr => tr.Id == id);

        if (routeInfos is null) return null;
        
        routeInfos.Scraped();
        
        var fahrt = await service.GetFahrtAsync(routeInfos.BahnJourneyId.Value);

        if (fahrt is null) return null;

        var stopEvas = fahrt.GetEvaNumbers();
        
        var existingStations = await context.Stations
            .AsNoTracking()
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionaryAsync(s => s.EvaNumber, s => s);

        var routeFactory = RouteFactory.Create(fahrt)
            .WithExistingInformation(routeInfos)
            .WithExistingStations(existingStations);
        
        await context.Stations.AddRangeAsync(routeFactory.StationsToCreate);

        return routeFactory.RouteDto;
    }
}