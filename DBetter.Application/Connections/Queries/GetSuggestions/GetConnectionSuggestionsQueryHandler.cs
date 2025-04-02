using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections;

namespace DBetter.Application.Connections.Queries.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(IConnectionsQueryRepository repository) : ICommandHandler<GetConnectionSuggestionsQuery, ConnectionSuggestionsDto>
{
    public async Task<CanFail<ConnectionSuggestionsDto>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        var result = ConnectionRequest.Create(
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