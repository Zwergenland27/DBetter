using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Application.Abstractions.Caching;
using DBetter.Domain.TrainCompositions.Events;

namespace DBetter.Application.TrainCompositions.Events;

public class TrainCompositionUpdatedCacheClear(ICache cache): DomainEventHandlerBase<TrainCompositionUpdated>
{
    public override Task<CanFail> Handle(TrainCompositionUpdated @event, CancellationToken cancellationToken)
    {
        cache.Remove($"composition:{@event.TrainRun.Value}");
        return Task.FromResult(CanFail.Success);
    }
}