using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Infrastructure.BahnDe.TrainCompositions.RealTime;
using Microsoft.Extensions.Http.Resilience;
using Polly;

namespace DBetter.Infrastructure.BahnDe.Departures;

public class DepartureProvider(HttpClient http)
{
    private ResiliencePipeline<HttpResponseMessage> _resiliencePipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
        .AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 4,
            Delay = TimeSpan.FromSeconds(5),
            ShouldHandle = BahnDeRetryStrategyOptions.IsTransient
        })
        .Build();
    public async Task<List<Abfahrt>> GetJourneyIds(EvaNumber evaNumber, DateTime at)
    {
        var url =
            $"reiseloesung/abfahrten?datum={at:yyyy-MM-dd}&zeit={at:HH:mm:ss}&ortExtId={evaNumber.Value}&verkehrsmittel[]=ICE&verkehrsmittel[]=EC_IC&verkehrsmittel[]=IR";
        var response = await _resiliencePipeline.ExecuteAsync(async ct => await http.GetAsync(url, ct));
        
        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("DepartureProvider", $"Instead of success the station API returned {response.StatusCode}");  
        }
            
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Response>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });
            
        if (result is null) throw new BahnDeException("DepartureProvider","No departure data received");
            
        if (result.Entries is null) return [];

        return result.Entries;
    }
}