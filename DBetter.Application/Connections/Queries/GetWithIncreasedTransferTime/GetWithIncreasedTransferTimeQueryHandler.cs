using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.Connections;
using DBetter.Domain.Errors;

namespace DBetter.Application.Connections.Queries.GetWithIncreasedTransferTime;

public class GetWithIncreasedTransferTimeQueryHandler(
    IConnectionsQueryRepository repository) : ICommandHandler<GetWithIncreasedTransferTimeQuery, ConnectionDto>
{
    public async Task<CanFail<ConnectionDto>> Handle(GetWithIncreasedTransferTimeQuery request, CancellationToken cancellationToken)
    {
        var connection = await repository.GetConnectionWithIncreasedTransferTime(
            request.Id,
            request.FixedStartEvaNumber,
            request.FixedStartTime,
            request.FixedEndEvaNumber,
            request.FixedEndTime);

        if(connection is null) return DomainErrors.Connection.NotFound(request.Id);

        return connection;
    }
}