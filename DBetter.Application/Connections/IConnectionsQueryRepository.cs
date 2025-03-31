using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Application.Connections;

public interface IConnectionsQueryRepository
{
    Task<List<Connection>> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page);
    
    Task<Connection?> GetConnectionWithIncreasedTransferTime(
        ConnectionId id,
        EvaNumber fixedStartEvaNumber,
        DepartureTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        ArrivalTime fixedEndTime);
}