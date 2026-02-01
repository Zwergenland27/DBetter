using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

public class Transfer : Entity<TransferIndex>
{
    public SubConnection PreviousSubConnection { get; private set; }
    
    public SubConnection FollowingSubConnection { get; private set; }

    private Transfer(
        TransferIndex id,
        SubConnection previousSubConnection,
        SubConnection followingSubConnection) : base(id)
    {
        PreviousSubConnection = previousSubConnection;
        FollowingSubConnection = followingSubConnection;
    }
    private Transfer() : base(null!){}

    internal static Transfer Create(
        TransferIndex transferIndex,
        MeansOfTransport previousMeansOfTransport,
        StationId firstStationId, DateTime firstStationDeparture,
        StationId previousStationId, DateTime previousStationArrival,
        MeansOfTransport followingMeansOfTransport,
        StationId followingStationId, DateTime followingStationDeparture,
        StationId lastStationId, DateTime lastStationArrival)
    {
        var previousSubConnection = new SubConnection(
            firstStationId, firstStationDeparture, previousStationId, previousStationArrival, previousMeansOfTransport);
        var followingSubConnection = new SubConnection(
            followingStationId, followingStationDeparture, lastStationId, lastStationArrival,
            followingMeansOfTransport);
        
        return new Transfer(transferIndex, previousSubConnection, followingSubConnection);
    }
}