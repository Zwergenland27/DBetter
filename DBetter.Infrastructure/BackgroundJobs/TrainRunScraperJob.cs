using System.Collections.Concurrent;
using DBetter.Application.TrainRuns.Queries.Get;
using DBetter.Domain.TrainRun.ValueObjects;
using DBetter.Infrastructure.Postgres;
using MediatR;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public class TrainRunScraperJob(
    IMediator mediator) : IJob
{
    public static JobKey JobKey => JobKey.Create(nameof(TrainRunScraperJob));
    
    private static readonly ConcurrentQueue<TrainRunId> _trainRunQueue = [];

    private static void AddTrainRunToScrape(TrainRunId trainRunId)
    {
        lock (_trainRunQueue)
        {
            _trainRunQueue.Enqueue(trainRunId);
        }
    }

    public static void AddTrainRuns(List<TrainRunId> trainRunIds)
    {
        trainRunIds.ForEach(AddTrainRunToScrape);
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        TrainRunId?  trainRunId;

        lock (_trainRunQueue)
        {
            if (!_trainRunQueue.TryDequeue(out trainRunId)) return;
        }
        
        _ = await mediator.Send(new GetTrainRunQuery(trainRunId));
    }
}

//