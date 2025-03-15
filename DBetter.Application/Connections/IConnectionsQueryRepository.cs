using DBetter.Domain.ConnectionRequests;

namespace DBetter.Application.Connections;

public interface IConnectionsQueryRepository
{
    Task<string> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page);
}