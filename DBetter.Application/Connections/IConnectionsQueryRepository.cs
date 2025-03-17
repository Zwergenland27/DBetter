using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;

namespace DBetter.Application.Connections;

public interface IConnectionsQueryRepository
{
    Task<List<Connection>> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page);
}