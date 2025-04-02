using DBetter.Application.TrainRuns;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes;
using DBetter.Infrastructure.BahnDe.TrainRuns;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class TrainRunQueryRepository(
    DBetterContext context,
    RouteService service) : ITrainRunQueryRepository
{
    public async Task<Route?> GetAsync(RouteId id)
    {
        var trainRunInfos = await context.Routes
            .FirstOrDefaultAsync(tr => tr.Id == id);

        if (trainRunInfos is null) return null;
        
        trainRunInfos.Scraped();
        
        var fahrt = await service.GetFahrtAsync(trainRunInfos.JourneyId.Value);

        if (fahrt is null) return null;

        var stopEvas = fahrt
            .Halte
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .Distinct();
        
        var existingStations = context.Stations
            .AsNoTracking()
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionary(s => s.EvaNumber.Value, s => s);

        
        var trainRun = fahrt.ToDomain(trainRunInfos, existingStations, out var stationsToCreate);
        
        await context.Stations.AddRangeAsync(stationsToCreate);

        return trainRun;
    }
}