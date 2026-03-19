using DBetter.Infrastructure.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DBetter.Infrastructure.OutboxPattern;

public static class DependencyInjection
{
    public static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new QuartzSettings();
        configuration.Bind(QuartzSettings.SectionName, settings);
        
        services.AddQuartz(options =>
        {
            options.AddJob<OutboxProcessor>(OutboxProcessor.JobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(OutboxProcessor.JobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));
            
            options.AddJob<ScrapeLongDistanceTrainsJob>(ScrapeLongDistanceTrainsJob.JobKey)
                .AddTrigger(trigger => trigger
                    .WithIdentity("ScrapeLongDistanceTrains")
                    .ForJob(ScrapeLongDistanceTrainsJob.JobKey)
                    .WithCronSchedule("0 0 3 * * ?", x => x.WithMisfireHandlingInstructionDoNothing()));
            
            options.UsePersistentStore(store =>
            {
                store.UsePostgres(settings.ConnectionString);
                store.UseSystemTextJsonSerializer();
            });
        });

        services.AddQuartzHostedService();

        return services;
    }
}