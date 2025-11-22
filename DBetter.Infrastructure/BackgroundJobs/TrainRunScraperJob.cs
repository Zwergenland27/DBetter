using System.Collections.Concurrent;
using DBetter.Application.Routes.Queries.Get;
using DBetter.Domain.Routes.ValueObjects;
using MediatR;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public class RouteScraperJob(
    IMediator mediator) : IJob
{
    public static JobKey JobKey => JobKey.Create(nameof(RouteScraperJob));
    
    private static readonly ConcurrentQueue<RouteId> _routeQueue = [];

    private static void AddRoutesToScrape(RouteId routeId)
    {
        lock (_routeQueue)
        {
            _routeQueue.Enqueue(routeId);
        }
    }

    public static void AddRoutes(List<RouteId> routeIds)
    {
        routeIds.ForEach(AddRoutesToScrape);
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        RouteId?  routeId;

        lock (_routeQueue)
        {
            if (!_routeQueue.TryDequeue(out routeId)) return;
        }
        
        _ = await mediator.Send(new GetRouteQuery(routeId));
    }
}

//