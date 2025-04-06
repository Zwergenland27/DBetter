using System.Collections.Concurrent;
using DBetter.Application.TrainRuns.Queries.Get;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.Postgres;
using MediatR;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public class RouteScraperJob(
    IMediator mediator) : IJob
{
    public static JobKey JobKey => JobKey.Create(nameof(RouteScraperJob));
    
    private static readonly ConcurrentQueue<RouteId> _trainRunQueue = [];

    private static void AddTrainRunToScrape(RouteId routeId)
    {
        lock (_trainRunQueue)
        {
            _trainRunQueue.Enqueue(routeId);
        }
    }

    public static void AddRoutes(List<RouteId> trainRunIds)
    {
        trainRunIds.ForEach(AddTrainRunToScrape);
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        RouteId?  trainRunId;

        lock (_trainRunQueue)
        {
            if (!_trainRunQueue.TryDequeue(out trainRunId)) return;
        }
        
        _ = await mediator.Send(new GetTrainRunQuery(trainRunId));
    }
}

//