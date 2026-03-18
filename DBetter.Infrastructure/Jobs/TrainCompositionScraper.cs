using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainRuns.ValueObjects;
using Quartz;

namespace DBetter.Infrastructure.Jobs;

public class TrainCompositionScraper(ISchedulerFactory schedulerFactory) : ITrainCompositionScraper
{
    public async Task ScheduleUpdate(TrainRunId trainRunId, DateTime scheduledDate)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        var jobKey = new JobKey($"trainComposition:{trainRunId.Value}-{scheduledDate}");

        if (await scheduler.CheckExists(jobKey)) return;
        
        var job = JobBuilder.Create<TrainCompositionUpdateJob>()
            .WithIdentity(jobKey)
            .UsingJobData(TrainCompositionUpdateJob.TrainRunIdPropertyMapName, trainRunId.Value)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithSimpleSchedule()
            .StartAt(scheduledDate)
            .Build();
        
        await scheduler.ScheduleJob(job, trigger);
    }
}