using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.Requests;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class BahnDeConnectionProvider(HttpClient http) : IExternalConnectionProvider
{
    public async Task<SuggestionResponse> GetSuggestions(SuggestionRequest request)
    {
        var anfrage = ConnectionRequestFactory.FromRequest(request)
            .Build();
        
        var response = await http.PostAsJsonAsync("angebote/fahrplan", anfrage, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("BahnDeConnectionProvider.GetSuggestions", $"Requesting suggestions from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var fahrplan = JsonSerializer.Deserialize<Fahrplan>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (fahrplan is null)
        {
            throw new BahnDeException("BahnDeConnectionProvider.GetSuggestions",
                "Response from bahn.de could not be parsed");
        }
        
        return new ConnectionSnapshotFactory(request, fahrplan).ExtractSnapshot();
    }
}