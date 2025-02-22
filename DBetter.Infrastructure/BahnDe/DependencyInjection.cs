using DBetter.Infrastructure.BahnDe.Stations;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.BahnDe;

public static class DependencyInjection
{
    public static IServiceCollection AddBahnApi(this IServiceCollection services)
    {
        services.AddHttpClient<StationService>(
            client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"));
        return services;
    }
}