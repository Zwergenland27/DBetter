using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class TrainRunRepository(DBetterContext db) : ITrainRunRepository
{
    public Task<TrainRun?> GetAsync(TrainRunId id)
    {
        return db.TrainRuns.FirstOrDefaultAsync(r => r.Id == id);
    }

    public Task<List<TrainRun>> GetManyAsync(IEnumerable<BahnJourneyId> journeyIds)
    {
        return db.TrainRuns.Where(r => journeyIds.Contains(r.JourneyId)).ToListAsync();
    }

    public void AddRange(IEnumerable<TrainRun> routes)
    {
        db.TrainRuns.AddRange(routes);
    }
}