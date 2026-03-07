using DBetter.Domain.TrainRuns.Events;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.OutboxPattern;
using DBetter.Infrastructure.Postgres;
using Quartz;

namespace DBetter.Infrastructure.Jobs;

public class DelayCheckScraperJob(DBetterContext db) : IJob
{
    public const string TrainRunIdPropertyMapName = "TrainRunId";
    public Task Execute(IJobExecutionContext context)
    {
        var trainRunId = context.JobDetail.JobDataMap.GetGuidValue(TrainRunIdPropertyMapName);
        var message = OutboxMessage.FromEvent(new TrainRunScrapingScheduledEvent(new TrainRunId(trainRunId)));
        
        db.OutboxMessages.Add(message);
        
        return db.SaveChangesAsync();
    }
}