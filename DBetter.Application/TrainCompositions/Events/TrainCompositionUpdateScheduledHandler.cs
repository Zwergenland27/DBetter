using CleanDomainValidation.Domain;
using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.Events;

namespace DBetter.Application.TrainCompositions.Events;

public class TrainCompositionUpdateScheduledHandler(ITrainCompositionScraper scraper) : DomainEventHandlerBase<TrainCompositionUpdateScheduled>
{
    public override async Task<CanFail> Handle(TrainCompositionUpdateScheduled @event, CancellationToken cancellationToken)
    {
        await scraper.ScheduleUpdate(@event.TrainRun, @event.ScheduledAt);
        return CanFail.Success;
    }
}