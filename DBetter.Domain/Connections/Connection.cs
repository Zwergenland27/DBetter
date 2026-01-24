using DBetter.Domain.Abstractions;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections;

public class Connection : AggregateRoot<ConnectionId>
{
    public DateOnly ConnectionDate { get; private init; }
    public ConnectionContextId ContextId { get; private init; }
    
    private Connection(
        ConnectionId id,
        ConnectionContextId contextId,
        DateOnly connectionDate) : base(id)
    {
        ContextId = contextId;
        ConnectionDate = connectionDate;
    }
    
    private Connection() : base(null!){}

    public static Connection Create(ConnectionContextId contextId, DateOnly connectionDate)
    {
        return new Connection(ConnectionId.CreateNew(), contextId, connectionDate);
    }
}