using System.Net;
using DBetter.Application.Requests;
using DBetter.Application.TrainRuns;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Stations;
using DBetter.Infrastructure.BahnDe.TrainRuns;
using DBetter.Infrastructure.Monitoring;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.BahnDe;

public static class DependencyInjection
{
    public static IServiceCollection AddBahnApi(this IServiceCollection services)
    {
        services.AddHttpClient<StationService>(client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            })
            .AddHttpMessageHandler<MetricHttpHandler>();
        
        services.AddHttpClient<IExternalConnectionProvider, BahnDeConnectionProvider>(
                client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            })
            .AddHttpMessageHandler<MetricHttpHandler>();
        
        services.AddHttpClient<IExternalTrainRunProvider, BahnDeTrainRunProvider>(
                client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            })
            .AddHttpMessageHandler<MetricHttpHandler>();
        return services;
    }
}