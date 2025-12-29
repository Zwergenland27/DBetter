using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMessageBus.Abstractions;
using CleanMessageBus.Abstractions.Attributes;
using DBetter.Application.Routes.Queries.Get;
using DBetter.Domain.Routes.Events;

namespace DBetter.Application.Routes.Events;

[Throttled(RequestInterval = 1000)]
public class RouteScrapeEventHandler(IMediator mediator) : DomainEventHandlerBase<RouteScrapingScheduledEvent>
{
    public override async Task<CanFail> Handle(RouteScrapingScheduledEvent @event, CancellationToken cancellationToken)
    {
        var routeResult = await mediator.ExecuteAsync(new GetRouteQuery(@event.Id), cancellationToken);
        if (routeResult.HasFailed)
        {
            return routeResult.Errors;
        }
        
        return CanFail.Success;
    }
}