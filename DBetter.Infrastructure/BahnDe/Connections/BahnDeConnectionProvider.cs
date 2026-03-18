using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.Requests.IncreaseTransferTime;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using Polly;

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

        var connectionSnapshotFactory = new ConnectionSnapshotFactory(request);
        
        var earlierToken = fahrplan.VerbindungReference.Earlier;
        var laterToken = fahrplan.VerbindungReference.Later;
        
        return new SuggestionResponse
        {
            Connections = fahrplan.Verbindungen.Select(connection => connectionSnapshotFactory.ExtractConnectionSnapshot(connection)).ToList(),
            EarlierRef = earlierToken is null ? null : PaginationReference.Create(earlierToken),
            LaterRef = laterToken is null ? null : PaginationReference.Create(laterToken)
        };
    }

    public async Task<ConnectionDto?> GetWithIncreasedTransferTime(IncreaseTransferTimeRequest request)
    {
        var anfrage = ConnectionRequestFactory.FromRequest(request.OriginalRequest)
            .BuildWithIncreasedTransferTime(request.OriginalConnectionContextId.Value, request.FixedSubConnection,
                request.Mode);

        var response = await http.PostAsJsonAsync("angebote/teilstrecke", anfrage, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (!response.IsSuccessStatusCode)
        {
            throw new BahnDeException("BahnDeConnectionProvider.GetWithIncreasedTransferTime", $"Requesting connection with increased transfer time from bahn.de failed with status code {response.StatusCode}");
        }
        
        var responseString = await response.Content.ReadAsStringAsync();
        var teilstrecke = JsonSerializer.Deserialize<Teilstrecke>(responseString, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });

        if (teilstrecke is null)
        {
            throw new BahnDeException("BahnDeConnectionProvider.GetWithIncreasedTransferTime",
                "Response from bahn.de could not be parsed");
        }
        
        return new ConnectionSnapshotFactory(request).ExtractConnectionSnapshot(teilstrecke.Verbindung);
    }
}