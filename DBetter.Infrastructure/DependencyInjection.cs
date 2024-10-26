using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddRepositories();
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }
}