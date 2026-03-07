using DBetter.Application.Abstractions.RealtimeNotification;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.RealtimeNotification;

public static class DependencyInjection
{
    public static IServiceCollection AddRealtimeNotification(this IServiceCollection services)
    {
        services.AddScoped<IRealtimeNotifier, SignalRNotifier>();
        services.AddSignalR();
        return services;
    }
}