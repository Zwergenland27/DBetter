using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.Snapshots;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionResponse
{
    public required List<ConnectionSnapshot> Connections { get; init; }
    
    public required PaginationReference? EarlierRef { get; init; }
    
    public required PaginationReference? LaterRef { get; init; }
}