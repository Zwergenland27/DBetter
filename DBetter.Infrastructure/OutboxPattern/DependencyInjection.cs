using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace DBetter.Infrastructure.OutboxPattern;

public static class DependencyInjection
{
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.AddJob<OutboxProcessor>(OutboxProcessor.JobKey)
                .AddTrigger(trigger => trigger
                    .ForJob(OutboxProcessor.JobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));
        });

        services.AddQuartzHostedService();

        return services;
    }
}