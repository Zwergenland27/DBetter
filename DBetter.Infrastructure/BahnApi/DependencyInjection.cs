using System.Net;
using DBetter.Infrastructure.BahnApi.Journey;
using DBetter.Infrastructure.BahnApi.VehicleSequence;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.BahnApi;

public static class DependencyInjection
{
    public static IServiceCollection AddBahnApi(this IServiceCollection services)
    {
        services.AddHttpClient<JourneyRepository>(
            client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
        
        services.AddHttpClient<VehicleSequenceRepository>(
            client => client.BaseAddress = new Uri("https://www.bahn.de/web/api/"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
        
        return services;
    }
}