using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.OutboxPattern;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.TrainCompositions;

public class TrainCompositionRepository(DBetterContext db) : ITrainCompositionRepository
{
    public void Save(TrainComposition trainComposition)
    {
        var existing = db.TrainCompositions.Local.FirstOrDefault(e => e.Id == trainComposition.Id.Value);
        if (existing is null)
        {
            db.TrainCompositions.Add(TrainCompositionPersistenceDto.FromDomain(trainComposition));
        }
        else
        {
            existing.Apply(trainComposition);
        }
        
        db.OutboxMessages.AddRange(trainComposition.DomainEvents.Select(OutboxMessage.FromEvent));
    }

    public async Task<TrainComposition?> GetAsync(TrainRunId trainRunId)
    {
        var dto = await db.TrainCompositions.FirstOrDefaultAsync(tc => tc.TrainRunId == trainRunId.Value);
        return dto?.ToDomain();
    }

    public async Task<List<TrainComposition>> GetAsync(IEnumerable<TrainRunId> trainRunIds)
    {
        var databaseFriendlyTrainRunIds = trainRunIds.Select(id => id.Value);
        var dtos = await db.TrainCompositions.Where(tc => databaseFriendlyTrainRunIds.Contains(tc.TrainRunId)).ToListAsync();

        return dtos.Select(dto => dto.ToDomain()).ToList();
    }
}