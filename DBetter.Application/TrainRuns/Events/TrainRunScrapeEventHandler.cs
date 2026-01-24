using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;
using DBetter.Application.TrainRuns.Queries.Get;
using DBetter.Domain.TrainRuns.Events;

namespace DBetter.Application.TrainRuns.Events;

[Throttled(RequestInterval = 1000)]
public class TrainRunScrapeEventHandler(IMediator mediator) : DomainEventHandlerBase<TrainRunScrapingScheduledEvent>
{
    public override async Task<CanFail> Handle(TrainRunScrapingScheduledEvent @event, CancellationToken cancellationToken)
    {
        var routeResult = await mediator.ExecuteAsync(new GetTrainRunQuery(@event.Id), cancellationToken);
        if (routeResult.HasFailed)
        {
            return routeResult.Errors;
        }
        
        return CanFail.Success;
    }
}