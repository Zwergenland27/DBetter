using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Domain.Routes.Events;
using DBetter.Domain.TrainRuns;

namespace DBetter.Application.TrainRuns.Events;

public class DelayCheckScheduledEventHandler(IDelayCheckScraper scraper) : DomainEventHandlerBase<DelayCheckScheduledEvent>
{
    public override async Task<CanFail> Handle(DelayCheckScheduledEvent @event, CancellationToken cancellationToken)
    {
        await scraper.ScheduleDelayCheck(@event.OfTrainRun, @event.ScheduledAt);
        return CanFail.Success;
    }
}