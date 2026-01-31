using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class TrainCirculationRepository(DBetterContext db) : ITrainCirculationRepository
{
    public Task<TrainCirculation?> GetAsync(TrainCirculationId id)
    {
        return db.TrainCirculations.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<TrainCirculation>> GetManyAsync(IEnumerable<TimeTableCompositeIdentifier> timeTableIdentifier)
    {
        var sql = $"""
                   SELECT *
                   FROM "TrainCirculations"
                   WHERE ("TrainId", "TimeTablePeriod") IN (
                       {string.Join(",", timeTableIdentifier.Select(id => $"({id.TrainId.Value}, {id.TimeTablePeriod.Year})"))}
                   )
                   """;
        
        return db.TrainCirculations
            .FromSqlRaw(sql)
            .ToListAsync();
    }

    public void AddRange(IEnumerable<TrainCirculation> trainCirculations)
    {
        db.TrainCirculations.AddRange(trainCirculations);
    }
}