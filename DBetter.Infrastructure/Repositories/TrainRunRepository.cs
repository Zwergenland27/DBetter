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

    public Task<TrainRun?> GetAsync(TrainRunIdentifier identifier)
    {
        throw new NotImplementedException("TrainRuns as persistence dtos and use composite identifier");
//         var sql = $"""
//                    SELECT tr.*
//                    FROM "TrainRuns" tr
//                    JOIN "TrainCirculations" tc ON tr."CirculationId" = tc."Id"
//                    WHERE 
//                        tc."TrainId" = {identifier.TrainId.Value} AND
//                        tc."TimeTablePeriod" = {identifier.TimeTablePeriod.Year} AND
//                        tr."OperatingDay" = '{identifier.OperatingDay.Date.ToString("yyyy-MM-dd")}'
//                    """;
//         
//         return db.TrainRuns
//             .FromSqlRaw(sql)
//             .FirstOrDefaultAsync();
    }

    public Task<List<TrainRun>> GetManyAsync(IEnumerable<TrainRunIdentifier> identifiers)
    {
        throw new NotImplementedException("TrainRuns as persistence dtos and use composite identifier");
//         var sql = $"""
//                    SELECT tr.*
//                    FROM "TrainRuns" tr
//                    JOIN "TrainCirculations" tc ON tr."CirculationId" = tc."Id"
//                    WHERE (tc."TrainId", tc."TimeTablePeriod", tr."OperatingDay") IN (
//                        {string.Join(",", identifiers.Select(id => $"({id.TrainId.Value}, {id.TimeTablePeriod.Year}, '{id.OperatingDay.Date.ToString("yyyy-MM-dd")}')"))}
//                    )
//                    """;
//         
//         return db.TrainRuns
//             .FromSqlRaw(sql)
//             .ToListAsync();
    }

    public void AddRange(IEnumerable<TrainRun> trainRuns)
    {
        db.TrainRuns.AddRange(trainRuns);
    }

    public void Add(TrainRun trainRun)
    {
        db.TrainRuns.Add(trainRun);
    }
}