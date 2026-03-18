using DBetter.Application.Abstractions.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddTransient<ICache, Cache>();
        return services;
    }
}