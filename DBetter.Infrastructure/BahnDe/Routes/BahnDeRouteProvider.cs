using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.Routes;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;

namespace DBetter.Infrastructure.BahnDe.Routes;

public class BahnDeRouteProvider(HttpClient http) : IExternalRouteProvider
{
    public async Task<RouteSnapshot> GetRouteAsync(BahnJourneyId journeyId)
    {
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
        
        return new RouteSnapshotFactory(fahrt).ExtractSnapshot();
    }
}