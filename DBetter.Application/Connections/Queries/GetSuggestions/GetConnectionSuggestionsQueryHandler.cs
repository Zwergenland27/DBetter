using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;

namespace DBetter.Application.Connections.Queries.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(IConnectionsQueryRepository repository) : IQueryHandler<GetConnectionSuggestionsQuery, List<Connection>>
{
    public async Task<CanFail<List<Connection>>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        var result = ConnectionRequest.Create(
            request.Id,
            request.OwnerId,
            request.DepartureTime,
            request.ArrivalTime,
            request.Passengers,
            request.Options,
            request.Route);

        if (result.HasFailed) return result.Errors;
        
        return await repository.GetConnectionSuggestionsAsync(result.Value, request.Page);
    }
}