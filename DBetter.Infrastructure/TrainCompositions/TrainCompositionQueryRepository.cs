using DBetter.Application.Abstractions.Caching;
using DBetter.Application.TrainCompositions;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.TrainCompositions;

public class TrainCompositionQueryRepository(DBetterContext db, ICache cache) : ITrainCompositionQueryRepository
{
    private record RawTrainCompositionResultDto(Guid TrainRunId, int Source, string Identifier, DateTime LastUpdate);
    
    public async Task<TrainCompositionResultDto?> GetAsync(TrainRunId trainRunId)
    {
        var cacheKey = $"composition:{trainRunId.Value}";

        if (cache.TryGetValue<TrainCompositionResultDto>(cacheKey, out var cached))
        {
            return cached;
        }
        var rawResults = await db.Database.SqlQuery<RawTrainCompositionResultDto>($"""
                                                                               SELECT tc."TrainRunId", tc."Source", v."Identifier", tc."LastUpdate"
                                                                               FROM "TrainCompositions" tc
                                                                               JOIN "FormationVehicles" fv ON tc."Id" = fv."TrainCompositionId"
                                                                               JOIN "Vehicles" v ON fv."VehicleId" = v."Id"
                                                                               WHERE tc."TrainRunId" = {trainRunId.Value}
                                                                               """).ToListAsync();
        var results = Map(rawResults);
        if (results is null)
        {
            cache.Set(cacheKey, results, new CachingOptions
            {
                Duration = TimeSpan.FromSeconds(20)
            });
        }
        else
        {
            cache.Set(cacheKey, results, new CachingOptions
            {
                Duration = TimeSpan.FromMinutes(5)
            });   
        }

        return results;
    }

    public async Task<List<TrainCompositionResultDto>> GetManyAsync(IEnumerable<TrainRunId> trainRunIds)
    {
        var results = new List<TrainCompositionResultDto>();

        foreach (var trainRunId in trainRunIds)
        {
            //TODO: ACHTUNG: n+1 Problem!
            var trainComposition = await GetAsync(trainRunId);
            if (trainComposition is null) continue;
            results.Add(trainComposition);
        }
        
        return results;
    }

    private static TrainCompositionResultDto? Map(List<RawTrainCompositionResultDto> rawResults)
    {
        if (!rawResults.Any()) return null;

        var id = rawResults.First().TrainRunId;
        var source = (TrainFormationSource) rawResults.First().Source;
        var lastUpdatedAt = rawResults.Last().LastUpdate;

        return new TrainCompositionResultDto
        {
            LastUpdatedAt = lastUpdatedAt,
            TrainRunId = id.ToString(),
            Source = source,
            Vehicles = rawResults.Select(r => r.Identifier).ToList()
        };
    }
}