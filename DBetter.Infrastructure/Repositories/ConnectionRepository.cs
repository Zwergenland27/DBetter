using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionRepository(DBetterContext db) : IConnectionRepository
{
    public Task<Connection?> GetAsync(ConnectionId connectionId)
    {
        return db.Connections.FirstOrDefaultAsync(connection => connection.Id == connectionId);
    }

    public Task<List<Connection>> GetManyAsync(IEnumerable<ConnectionContextId> contextIds)
    {
        return db.Connections.Where(connection => contextIds.Contains(connection.ContextId)).ToListAsync();
    }

    public void AddRange(IEnumerable<Connection> connections)
    {
        db.Connections.AddRange(connections);
    }
}