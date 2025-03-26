using System.Text.Json;
using DBetter.Application.TrainRuns;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.BahnDe.TrainRuns;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class TrainRunQueryRepository(
    DBetterContext context,
    TrainRunService service) : ITrainRunQueryRepository
{
    public async Task<TrainRun?> GetAsync(TrainRunId id)
    {
        var trainRunInfos = await context.TrainRuns
            .FirstOrDefaultAsync(tr => tr.Id == id);

        if (trainRunInfos is null) return null;
        
        var fahrt = await service.GetFahrtAsync(trainRunInfos.JourneyId.Value);

        if (fahrt is null) return null;

        var stopEvas = fahrt
            .Halte
            .Select(h => EvaNumber.Create(h.ExtId).Value)
            .Distinct();
        
        var existingStations = context.Stations
            .Where(s => stopEvas.Contains(s.EvaNumber))
            .ToDictionary(s => s.EvaNumber.Value, s => s);

        
        //TODO: Update train number of entity
        var trainRun = fahrt.ToDomain(trainRunInfos, existingStations, out var stationsToCreate);

        await context.Stations.AddRangeAsync(stationsToCreate);

        return trainRun;
    }
}