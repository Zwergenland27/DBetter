using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;

namespace DBetter.Application.Connections.Commands.CreateRequest;

public class CreateConnectionRequestCommandHandler(IConnectionsQueryRepository repository) : ICommandHandler<CreateConnectionRequestCommand, ConnectionSuggestionsDto>
{
    public async Task<CanFail<ConnectionSuggestionsDto>> Handle(CreateConnectionRequestCommand request, CancellationToken cancellationToken)
    {
        var result = ConnectionRequest.Create(
            request.OwnerId,
            request.DepartureTime,
            request.ArrivalTime,
            request.Passengers,
            request.ComfortClass,
            request.Route);

        if (result.HasFailed) return result.Errors;
        
        return await repository.GetConnectionSuggestionsAsync(result.Value);
    }
}