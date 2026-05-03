using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Queries;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;
using DBetter.Application.TrainCompositions.Get;
using DBetter.Domain.Errors;
using DBetter.Domain.TrainCompositions.Events;

namespace DBetter.Application.TrainCompositions.Events;

[Throttled(RequestInterval = 3000)]
[ConsumedBy(Name = "TrainCompositionUpdater")]
public class TrainCompositionUpdateRequestedHandler(IMediator mediator) : DomainEventHandlerBase<TrainCompositionUpdateRequested>
{
    public override async Task<CanFail> Handle(TrainCompositionUpdateRequested @event, CancellationToken cancellationToken)
    {
        var result = await mediator.RunAsync(new GetTrainCompositionQuery(@event.TrainRun), cancellationToken);
        if (result.HasFailed)
        {
            // if(result.Errors.Count == 1 && result.Errors.First() == DomainErrors.TrainComposition.NotFound || result.Errors.First() == DomainErrors.TrainComposition.NotFindable) return CanFail.Success;
            return result.Errors;
        }
        return CanFail.Success;
    }
}