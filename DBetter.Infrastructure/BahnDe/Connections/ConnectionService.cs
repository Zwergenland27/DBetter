using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class ConnectionService(HttpClient http)
{
    public async Task<Fahrplan> GetSuggestionsAsync(ReiseAnfrage request)
    {
        var rq = JsonSerializer.Serialize(request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        });
        
        var response = await http.PostAsJsonAsync("angebote/fahrplan", request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("ConnectionService.GetSuggestions", $"Requesting suggestions from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var fahrplan = JsonSerializer.Deserialize<Fahrplan>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (fahrplan is null)
        {
            throw new BahnDeException("ConnectionService.GetSuggestions",
                "Response from bahn.de could not be parsed");
        }

        return fahrplan;
    }

    public async Task<Teilstrecke> GetSuggestionsWithIncreasedTransferTimeAsync(TeilstreckeAnfrage request)
    {
        var response = await http.PostAsJsonAsync("angebote/teilstrecke", request, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("ConnectionService.GetWithIncreasedTransferTime", $"Requesting increased transfer time from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var teilstrecke = JsonSerializer.Deserialize<Teilstrecke>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (teilstrecke is null)
        {
            throw new BahnDeException("ConnectionService.GetWithIncreasedTransferTime", "Response from bahn.de could not be parsed");
        }

        return teilstrecke;
    } 
}