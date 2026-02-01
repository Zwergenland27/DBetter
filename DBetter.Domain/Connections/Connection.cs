using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Connections;

public class Connection : AggregateRoot<ConnectionId>
{
    private List<Transfer> _transfers = [];
    public DateOnly ConnectionDate { get; private init; }
    public ConnectionContextId ContextId { get; private init; }
    
    public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();
    
    private Connection(
        ConnectionId id,
        ConnectionContextId contextId,
        DateOnly connectionDate,
        List<Transfer> transfers) : base(id)
    {
        ContextId = contextId;
        ConnectionDate = connectionDate;
        _transfers = transfers;
    }
    
    private Connection() : base(null!){}

    public static Connection Create(
        ConnectionContextId contextId,
        DateOnly connectionDate,
        List<SegmentSnapshot> segments,
        Route requestedRoute)
    {
        var transfersFactory = new TransfersFactory(segments, requestedRoute.MeansOfTransportFirstSection,  requestedRoute.FirstStopover, requestedRoute.SecondStopover);
        
        return new Connection(ConnectionId.CreateNew(), contextId, connectionDate, transfersFactory.Extract());
    }

    public List<StationId> GetRequestedStationIds()
    {
        var stationIds = new List<StationId>();

        foreach (var transfer in _transfers)
        {
            stationIds.Add(transfer.PreviousSubConnection.FromStationId);
            stationIds.Add(transfer.PreviousSubConnection.ToStationId);
            stationIds.Add(transfer.FollowingSubConnection.FromStationId);
            stationIds.Add(transfer.FollowingSubConnection.ToStationId);
        }
        
        return stationIds;
    }
}