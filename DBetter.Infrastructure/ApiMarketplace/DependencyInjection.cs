using System.Net;
using DBetter.Infrastructure.ApiMarketplace.StaDa;
using DBetter.Infrastructure.ApiMarketplace.Timetables;
using DBetter.Infrastructure.Monitoring;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DBetter.Infrastructure.ApiMarketplace;

public static class DependencyInjection
{
    public static IServiceCollection AddApiMarketplace(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = new ApiMarketplaceSettings();
        configuration.Bind(ApiMarketplaceSettings.SectionName, settings);
        
        services.AddHttpClient<StaDaService>(client =>
            {
                client.BaseAddress =
                    new Uri("https://apis.deutschebahn.com/db-api-marketplace/apis/station-data/v2/");
                client.DefaultRequestHeaders.Add("DB-Client-ID", settings.ClientId);
                client.DefaultRequestHeaders.Add("DB-Api-Key", settings.ApiKey);
                
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            })
            .AddHttpMessageHandler<MetricHttpHandler>();
        
        services.AddHttpClient<TimetablesService>(client =>
            {
                client.BaseAddress =
                    new Uri("https://apis.deutschebahn.com/db-api-marketplace/apis/timetables/v1/");
                client.DefaultRequestHeaders.Add("DB-Client-ID", settings.ClientId);
                client.DefaultRequestHeaders.Add("DB-Api-Key", settings.ApiKey);
                
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            })
            .AddHttpMessageHandler<MetricHttpHandler>();
        
        return services;
    }
}