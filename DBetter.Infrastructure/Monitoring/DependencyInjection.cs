using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DBetter.Infrastructure.Monitoring;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddMonitoring(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<BahnApiMetrics>();
        builder.Logging.ClearProviders();
        var otel = builder.Services.AddOpenTelemetry();
            
        otel.ConfigureResource(resource => resource.AddService("DBetter.Api"));
        
        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddHttpClientInstrumentation();
            metrics.AddPrometheusExporter();
            metrics.AddMeter(BahnApiMetrics.Metername);
        });
        
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation(options =>
            {
                options.Filter = (context) => !context.Request.Path.StartsWithSegments("/metrics");
            });
            tracing.AddHttpClientInstrumentation();
            tracing.AddOtlpExporter();
        });
        return builder;
    }

    public static WebApplication UseMonitoring(this WebApplication app)
    {
        app.MapPrometheusScrapingEndpoint();
        return app;
    }
}