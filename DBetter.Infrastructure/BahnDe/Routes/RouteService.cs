using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;

namespace DBetter.Infrastructure.BahnDe.Routes;

public class RouteService(HttpClient http)
{
    public Task<Fahrt?> GetFahrtAsync(string journeyId)
    {
        var escapedJourneyId = Uri.EscapeDataString(journeyId);
        return http.GetFromJsonAsync<Fahrt>($"reiseloesung/fahrt?journeyId={escapedJourneyId}", new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        });
    }
}