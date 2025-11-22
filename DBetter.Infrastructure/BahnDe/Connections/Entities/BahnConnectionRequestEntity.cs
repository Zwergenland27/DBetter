using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections.Entities;

public class BahnConnectionRequestEntity
{
    public ConnectionRequestId Id { get; private init; }
    
    public ReiseAnfrage Request { get; private init; }

    public BahnConnectionRequestEntity(
        ConnectionRequestId id,
        ReiseAnfrage request)
    {
        Id = id;
        Request = request;
    }
}