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

    public Task<List<TrainRun>> GetManyAsync(IEnumerable<TrainRunCompositeIdentifier> compositeIdentifiers)
    {
        
        var sql = $"""
                   SELECT tr.*
                   FROM "TrainRuns" tr
                   JOIN "TrainCirculations" tc ON tr."CirculationId" = tc."Id"
                   WHERE (tc."TrainId", tc."TimeTablePeriod", tr."OperatingDay") IN (
                       {string.Join(",", compositeIdentifiers.Select(id => $"({id.TrainId.Value}, {id.TimeTablePeriod.Year}, '{id.OperatingDay.Date.ToString("yyyy-MM-dd")}')"))}
                   )
                   """;
        
        return db.TrainRuns
            .FromSqlRaw(sql)
            .ToListAsync();
    }

    public void AddRange(IEnumerable<TrainRun> trainRuns)
    {
        db.TrainRuns.AddRange(trainRuns);
    }
}