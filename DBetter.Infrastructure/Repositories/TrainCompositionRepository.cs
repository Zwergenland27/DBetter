using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class TrainCompositionRepository(DBetterContext db) : ITrainCompositionRepository
{
    public void Add(TrainComposition trainComposition)
    {
        db.Add(trainComposition);
    }

    public Task<TrainComposition?> GetAsync(TrainRunId trainRunId)
    {
         return db.TrainCompositions.FirstOrDefaultAsync(tc => tc.TrainRun == trainRunId);
    }

    public Task<List<TrainComposition>> GetAsync(IEnumerable<TrainRunId> trainRunIds)
    {
        return db.TrainCompositions.Where(tc => trainRunIds.Contains(tc.TrainRun)).ToListAsync();
    }
}