using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DBetter.Infrastructure.BackgroundJobs;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options
                .AddJob<TrainRunScraperJob>(TrainRunScraperJob.JobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(TrainRunScraperJob.JobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(1).RepeatForever())
                );
            
            options.AddJob<DatabaseTrainRunScrapingScheduler>(DatabaseTrainRunScrapingScheduler.JobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(DatabaseTrainRunScrapingScheduler.JobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(5).RepeatForever())
                    );
        });

        services.AddQuartzHostedService();
        return services;
    }
}