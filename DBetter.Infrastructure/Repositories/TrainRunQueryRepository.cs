using System.Text.Json;
using DBetter.Application.TrainRuns;
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
        
        Console.WriteLine(JsonSerializer.Serialize(fahrt));
        
        //TODO: Update trainRunInfos (for example train number)
        
        return fahrt.ToDomain(trainRunInfos);
    }
}