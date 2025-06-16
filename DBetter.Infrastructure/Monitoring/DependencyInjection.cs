using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DBetter.Infrastructure.Monitoring;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddMonitoring(this IHostApplicationBuilder builder)
    {
        var otelSettings = new OtelSettings();
        builder.Configuration.Bind(OtelSettings.SectionName, otelSettings); 
        
        builder.Logging.AddOpenTelemetry(config =>
        {
            config.IncludeScopes = true;
            config.IncludeFormattedMessage = true;
        });
        
        builder.Services.AddTransient<MetricHttpHandler>();
        
        var otel = builder.Services.AddOpenTelemetry();
            
        otel.ConfigureResource(resource => resource.AddService("DBetter.Api"));
        
        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddMeter(MetricHttpHandler.Metername);
            metrics.AddPrometheusExporter();
        });
        
        otel.WithTracing(tracing =>
        {
            tracing.AddHttpClientInstrumentation();
            tracing.AddEntityFrameworkCoreInstrumentation();
            tracing.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(otelSettings.Endpoint);
            });
        });
        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        app.MapPrometheusScrapingEndpoint().DisableHttpMetrics();
        return app;
    }
}