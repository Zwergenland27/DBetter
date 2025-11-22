using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Application.Requests;

public interface IConnectionSuggestionService
{
    Task<ConnectionDto> GetConnectionSuggestionsAsync(
        ConnectionRequest request,
        SuggestionMode suggestionMode);
}

public class ConnectionDto
{
    public required List<ConnectionResponse> Connections { get; set; }
    
    public required string? EarlierRef { get; set; }
    
    public required string? LaterRef { get; set; }
}