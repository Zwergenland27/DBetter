using DBetter.Domain.TrainRuns;
using DBetter.Domain.TrainRuns.ValueObjects;
using Quartz;

namespace DBetter.Infrastructure.Jobs;

public class DelayCheckScraper(ISchedulerFactory schedulerFactory) : IDelayCheckScraper
{
    public async Task ScheduleDelayCheck(TrainRunId trainRunId, DateTime scheduledDate)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        var jobKey = new JobKey($"trainRun:{trainRunId.Value}");

        if (await scheduler.CheckExists(jobKey))
        {
            await scheduler.DeleteJob(jobKey);
        }
        
        var job = JobBuilder.Create<DelayCheckScraperJob>()
            .WithIdentity(jobKey)
            .UsingJobData(DelayCheckScraperJob.TrainRunIdPropertyMapName, trainRunId.Value)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithSimpleSchedule()
            .StartAt(scheduledDate)
            .Build();
        
        await scheduler.ScheduleJob(job, trigger);
    }
}