using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.Snapshots;
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

    public Task<List<TrainRun>> GetManyAsync(IEnumerable<BahnJourneyId> jouneyIds)
    {
        return db.TrainRuns.Where(tr => jouneyIds.Contains(tr.JourneyId)) .ToListAsync();   
    }

    public void AddRange(IEnumerable<TrainRun> trainRuns)
    {
        db.TrainRuns.AddRange(trainRuns);
    }
}