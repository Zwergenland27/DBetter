using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Domain.ConnectionRequests;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions.DTOs;

namespace DBetter.Infrastructure.BahnDe.ConnectionSuggestions;

public class ConnectionSuggestionService(HttpClient http)
{
    public async Task<Fahrplan> GetSuggestionsAsync(ConnectionRequest connectionRequest, string? page)
    {
        var request = connectionRequest.ToRequest(page);
        
        var response = await http.PostAsJsonAsync("angebote/fahrplan", request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("ConnectionSuggestion", $"Requesting suggestions from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var fahrplan = JsonSerializer.Deserialize<Fahrplan>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (fahrplan is null)
        {
            throw new BahnDeException("ConnectionSuggestion",
                $"Response from bahn.de could not be parsed");
        }

        return fahrplan;
    }
}