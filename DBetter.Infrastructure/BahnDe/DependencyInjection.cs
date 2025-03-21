using System.Net;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Stations;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.BahnDe;

public static class DependencyInjection
{
    public static IServiceCollection AddBahnApi(this IServiceCollection services)
    {
        services.AddHttpClient<StationService>(
                client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
        
        services.AddHttpClient<ConnectionService>(
                client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
        return services;
    }
}