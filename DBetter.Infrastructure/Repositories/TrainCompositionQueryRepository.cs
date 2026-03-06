using DBetter.Application.TrainCompositions;
using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DBetter.Infrastructure.Repositories;

public class TrainCompositionQueryRepository(DBetterContext db, IMemoryCache cache) : ITrainCompositionQueryRepository
{
    private record RawTrainCompositionResultDto(Guid TrainRun, int Source, string Identifier);
    
    public async Task<TrainCompositionResultDto?> GetAsync(TrainRunId trainRunId)
    {
        var cacheKey = $"composition:{trainRunId.Value}";

        if (cache.TryGetValue<TrainCompositionResultDto>(cacheKey, out var cached))
        {
            return cached;
        }
        var rawResults = await db.Database.SqlQuery<RawTrainCompositionResultDto>($"""
                                                                               SELECT tc."TrainRun", tc."Source", v."Identifier"
                                                                               FROM "TrainCompositions" tc
                                                                               JOIN "FormationVehicles" fv ON tc."Id" = fv."TrainCompositionId"
                                                                               JOIN "Vehicles" v ON fv."VehicleId" = v."Id"
                                                                               WHERE tc."TrainRun" = {trainRunId.Value}
                                                                               """).ToListAsync();
        var results = Map(rawResults);
        if (results is null)
        {
            cache.Set(cacheKey, results, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20)
            });
        }
        else
        {
            cache.Set(cacheKey, results, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
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

        var id = rawResults.First().TrainRun;
        var source = (TrainFormationSource) rawResults.First().Source;

        return new TrainCompositionResultDto
        {
            TrainRunId = id.ToString(),
            Source = source,
            Vehicles = rawResults.Select(r => r.Identifier).ToList()
        };
    }
}