using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using TravelTime = DBetter.Domain.Routes.ValueObjects.TravelTime;

namespace DBetter.Application.Connections;

public interface IConnectionsQueryRepository
{
    Task<ConnectionSuggestionsDto> GetConnectionSuggestionsAsync(ConnectionRequest request, string? page);
    
    Task<ConnectionDto?> GetConnectionWithIncreasedTransferTime(
        ConnectionId id,
        EvaNumber fixedStartEvaNumber,
        TravelTime fixedStartTime,
        EvaNumber fixedEndEvaNumber,
        TravelTime fixedEndTime);
}