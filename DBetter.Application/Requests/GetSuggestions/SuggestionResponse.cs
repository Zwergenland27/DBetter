using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionResponse
{
    public required List<ConnectionDto> Connections { get; init; }
    
    public required PaginationReference? EarlierRef { get; init; }
    
    public required PaginationReference? LaterRef { get; init; }
}