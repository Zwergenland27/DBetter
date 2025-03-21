using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Domain.Connections;
using DBetter.Domain.Errors;

namespace DBetter.Application.Connections.Queries.GetWithIncreasedTransferTime;

public class GetWithIncreasedTransferTimeQueryHandler(
    IConnectionsQueryRepository repository) : ICommandHandler<GetWithIncreasedTransferTimeQuery, Connection>
{
    public async Task<CanFail<Connection>> Handle(GetWithIncreasedTransferTimeQuery request, CancellationToken cancellationToken)
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