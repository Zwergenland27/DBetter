using DBetter.Application.Requests.Snapshots;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionResponse
{
    public required List<ConnectionSnapshot> Connections { get; init; }
    
    public required PaginationReference? EarlierRef { get; init; }
    
    public required PaginationReference? LaterRef { get; init; }
}