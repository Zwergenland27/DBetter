using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Connections.Entities;

public class ConnectionEntity
{
    public ConnectionId Id { get; private init; }
    
    public ConnectionRequestId RequestId { get; private init; }
    
    public string ContextId { get; private init; }
    
    private ConnectionEntity(){}

    public ConnectionEntity(
        ConnectionId id,
        ConnectionRequestId requestId,
        string contextId)
    {
        Id = id;
        RequestId = requestId;
        ContextId = contextId;
    }
}