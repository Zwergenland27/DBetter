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
                .AddJob<RouteScraperJob>(RouteScraperJob.JobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(RouteScraperJob.JobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(1).RepeatForever())
                );
            
            options.AddJob<DatabaseRoutesScrapingScheduler>(DatabaseRoutesScrapingScheduler.JobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(DatabaseRoutesScrapingScheduler.JobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInMinutes(5).RepeatForever())
                    );
        });

        services.AddQuartzHostedService();
        return services;
    }
}