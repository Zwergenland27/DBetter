using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainCompositions.Events;

namespace DBetter.Application.TrainCompositions.Events;

public class TrainCompositionUpdatedRealtimeNotifier : DomainEventHandlerBase<TrainCompositionUpdated>
{
    public override Task<CanFail> Handle(TrainCompositionUpdated @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}