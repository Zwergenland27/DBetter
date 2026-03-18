using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;
using DBetter.Application.Stations.ScrapeDepartures;
using DBetter.Domain.Stations.Events;

namespace DBetter.Application.Stations.Events;

[Throttled(RequestInterval = 60000)]
public class ScrapeStationDeparturesEventHandler(IMediator mediator): DomainEventHandlerBase<StationDeparturesScrapingScheduledEvent>
{
    public override Task<CanFail> Handle(StationDeparturesScrapingScheduledEvent @event, CancellationToken cancellationToken)
    {
        return mediator.ExecuteAsync(new ScrapeStationDeparturesCommand(@event.StationId, @event.ForDay),cancellationToken); 
    }
}