using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections;

public interface IConnectionRepository
{ 
    Task<Connection?> GetAsync(ConnectionId connectionId);

    Task<List<Connection>> GetManyAsync(IEnumerable<ConnectionContextId> contextIds);
    
    void AddRange(IEnumerable<Connection> connections);
}