using DBetter.Domain.Abstractions;
using DBetter.Domain.Connections.Snapshots;
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

    public static Connection CreateFromSnapshot(ConnectionSnapshot snapshot)
    {
        var firstStation = snapshot.Segments
            .OfType<TransportSegmentSnapshot>()
            .First()
            .Stops
            .First();
        
        return new Connection(ConnectionId.CreateNew(), snapshot.ContextId, DateOnly.FromDateTime(firstStation.DepartureTime!.Planned));
    }
}