using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DBetter.Infrastructure.Monitoring;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddMonitoring(this IHostApplicationBuilder builder)
    {
        //builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.AddOtlpExporter();
        });
        
        builder.Services.AddTransient<MetricHttpHandler>();
        
        var otel = builder.Services.AddOpenTelemetry();
            
        otel.ConfigureResource(resource => resource.AddService("DBetter.Api"));
        
        otel.WithMetrics(metrics =>
        {
            metrics.AddAspNetCoreInstrumentation();
            metrics.AddMeter(MetricHttpHandler.Metername);
            metrics.AddOtlpExporter();
        });
        
        otel.WithTracing(tracing =>
        {
            tracing.AddHttpClientInstrumentation();
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddEntityFrameworkCoreInstrumentation();
            tracing.AddOtlpExporter();
        });
        return builder;
    }
}