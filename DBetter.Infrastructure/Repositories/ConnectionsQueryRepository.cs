using System.Text.Json;
using System.Text.Json.Serialization;
using DBetter.Application.Connections;
using DBetter.Domain.ConnectionRequests;
using DBetter.Infrastructure.BahnDe.ConnectionSuggestions;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionsQueryRepository(ConnectionSuggestionService connectionSuggestionService) : IConnectionsQueryRepository
{
    public async Task<string> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page)
    {
        var response = await connectionSuggestionService.GetSuggestionsAsync(request, page);

        return JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new  JsonStringEnumConverter()}
        });
    }
}