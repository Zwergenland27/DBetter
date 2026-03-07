using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.Abstractions.Caching;
using DBetter.Application.TrainRuns;
using DBetter.Application.TrainRuns.Dtos;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

namespace DBetter.Infrastructure.BahnDe.TrainRuns;

public class BahnDeTrainRunProvider(
    ICache cache,
    HttpClient http) : IExternalTrainRunProvider
{
    public async Task<TrainRunDto> GetTrainRunAsync(BahnJourneyId journeyId)
    {
        if (cache.TryGetValue<TrainRunDto>($"trainRunProvider:get:{journeyId}", out var trainRunDto))
        {
            return trainRunDto;
        }
        var escapedJourneyId = Uri.EscapeDataString(journeyId.Value);
        var response = await http.GetAsync($"reiseloesung/fahrt?journeyId={escapedJourneyId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("BahnDeRouteProvider.GetRoute", $"Requesting route from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var fahrt = JsonSerializer.Deserialize<Fahrt>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (fahrt is null)
        {
            throw new BahnDeException("BahnDeRouteProvider.GetRoute",
                "Response from bahn.de could not be parsed");
        }
        
        var result = new RouteSnapshotFactory(fahrt).ExtractSnapshot();

        cache.Set($"trainRunProvider:get:{journeyId}", result, new CachingOptions
        {
            Duration = TimeSpan.FromSeconds(15)
        });
        
        return result;
    }
}