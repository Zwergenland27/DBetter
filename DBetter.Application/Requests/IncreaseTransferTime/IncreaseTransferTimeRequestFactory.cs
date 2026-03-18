using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations;

namespace DBetter.Application.Requests.IncreaseTransferTime;

public class IncreaseTransferTimeRequestFactory(
    ConnectionRequest connectionRequest,
    Connection originalConnection,
    List<Station> requestedStations)
{
    public IncreaseTransferTimeRequest Build(Transfer transfer, IncreaseTransferTimeMode mode = IncreaseTransferTimeMode.ArriveEarlier)
    {
        var subConnection = mode switch
        {
            IncreaseTransferTimeMode.ArriveEarlier => transfer.FollowingSubConnection,
            IncreaseTransferTimeMode.DepartLater => transfer.PreviousSubConnection,
            _ => throw new ArgumentOutOfRangeException(nameof(mode))
        };
        
        var originalRequestBuilder = new SuggestionRequestFactory(connectionRequest, requestedStations);
        return new IncreaseTransferTimeRequest
        {
            Mode = mode,
            FixedSubConnection = MapSubConnection(subConnection),
            OriginalRequest = originalRequestBuilder.Build(),
            OriginalConnectionContextId = originalConnection.ContextId
        };
    }

    private FixedSubConnection MapSubConnection(SubConnection subConnection)
    {
        return new FixedSubConnection
        {
            StartEvaNumber = requestedStations.First(s => s.Id == subConnection.FromStationId).EvaNumber,
            StartTime = subConnection.DepartureTime,
            EndEvaNumber = requestedStations.First(s => s.Id == subConnection.ToStationId).EvaNumber,
            EndTime = subConnection.ArrivalTime,
            MeansOfTransport = subConnection.OriginalMeansOfTransport
        };
    }
}