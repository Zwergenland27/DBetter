using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using DBetter.Infrastructure.TrainCirculations;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.TrainRuns;

public class TrainRunRepository(DBetterContext db) : ITrainRunRepository
{
    private record DatabaseFriendlyTrainRunIdentifier(DateOnly OperatingDay, string TrainCirculationIdentifier);
    
    public async Task<TrainRun?> GetAsync(TrainRunId id)
    {
        var dto = await db.TrainRuns.FirstOrDefaultAsync(r => r.Id == id.Value);
        return dto?.ToDomain();
    }

    public async Task<TrainRun?> GetAsync(TrainRunIdentifier identifier)
    {
        var databaseFriendlyIdentifier = new DatabaseFriendlyTrainRunIdentifier(identifier.OperatingDay.Date, identifier.TrainCirculation.DatabaseFriendly());
        
        var result = await db.TrainRuns
            .Join(
                db.TrainCirculations,
                trainRun => trainRun.TrainCirculationId,
                trainCirculation => trainCirculation.Id,
                (trainRun, trainCirculation) => new {TrainRun = trainRun, TrainCirculation = trainCirculation})
            .Where(x => x.TrainRun.OperatingDay == databaseFriendlyIdentifier.OperatingDay && x.TrainCirculation.Identifier == databaseFriendlyIdentifier.TrainCirculationIdentifier)
            .Select(x => x.TrainRun)
            .FirstOrDefaultAsync();

        return result?.ToDomain();
    }

    public async Task<List<TrainRun>> GetManyAsync(IEnumerable<TrainRunIdentifier> identifiers)
    { 
        var databaseFriendlyIdentifiers = identifiers.Select(x => new DatabaseFriendlyTrainRunIdentifier(x.OperatingDay.Date, x.TrainCirculation.DatabaseFriendly()));
        var sql = $"""
                   SELECT tr.*
                   FROM "TrainRuns" tr
                   JOIN "TrainCirculations" tc ON tr."TrainCirculationId" = tc."Id"
                   WHERE (tc."Identifier", tr."OperatingDay") IN (
                       {string.Join(",", databaseFriendlyIdentifiers.Select(id => $"('{id.TrainCirculationIdentifier}', '{id.OperatingDay.ToString("yyyy-MM-dd")}')"))}
                   )
                   """;
         
        var results = await db.TrainRuns
            .FromSqlRaw(sql)
            .ToListAsync();
        return results.Select(r => r.ToDomain()).ToList();
    }

    public void Save(IEnumerable<TrainRun> trainRuns)
    {
        var locals = db.TrainRuns.Local.ToList();
        var newTrainCirculations = new List<TrainRunPersistenceDto>();
        foreach (var trainRun in trainRuns)
        {
            var existing = locals.FirstOrDefault(e => e.Id == trainRun.Id.Value);
            if (existing is null)
            {
                newTrainCirculations.Add(TrainRunPersistenceDto.FromDomain(trainRun));
            }
            else
            {
                existing.Apply(trainRun);
            }
        }
        db.TrainRuns.AddRange(newTrainCirculations);
    }

    public void Save(TrainRun trainRun)
    {
        var existing = db.TrainRuns.Local.FirstOrDefault(e => e.Id == trainRun.Id.Value);
        if (existing is null)
        {
            db.TrainRuns.Add(TrainRunPersistenceDto.FromDomain(trainRun));
        }
        else
        {
            existing.Apply(trainRun);
        }
    }
}