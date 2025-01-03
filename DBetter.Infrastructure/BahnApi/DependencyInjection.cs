using DBetter.Infrastructure.BahnApi.Journey;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.BahnApi;

public static class DependencyInjection
{
    public static IServiceCollection AddBahnApi(this IServiceCollection services)
    {
        services.AddHttpClient<JourneyRepository>(
            client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"));
        
        return services;
    }
}