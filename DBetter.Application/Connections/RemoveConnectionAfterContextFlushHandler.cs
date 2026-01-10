using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Domain.ConnectionRequests.Events;
using DBetter.Domain.Connections;

namespace DBetter.Application.Connections;

public class RemoveConnectionAfterContextFlushHandler(
    IUnitOfWork unitOfWork,
    IConnectionRepository connectionRepository) : DomainEventHandlerBase<ConnectionContextFlushedEvent>
{
    public override async Task<CanFail> Handle(ConnectionContextFlushedEvent @event, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        
        var connection =  await connectionRepository.GetAsync(@event.ConnectionId);
        if (connection is null) return CanFail.Success;
        connectionRepository.Remove(connection);

        await unitOfWork.CommitAsync(cancellationToken);
        
        return CanFail.Success;
    }
}