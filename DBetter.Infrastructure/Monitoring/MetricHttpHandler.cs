using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace DBetter.Infrastructure.Monitoring;

public class MetricHttpHandler : DelegatingHandler
{
    public const string Metername = "BahnDe.Web.Api";
    
    private readonly Histogram<double> _durationHistogram;
    private readonly Counter<long> _requestCounter;

    public MetricHttpHandler(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(Metername);
        _durationHistogram = meter.CreateHistogram<double>("http_client_duration", unit: "ms");
        _requestCounter = meter.CreateCounter<long>("http_client_requests");
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage? response = null;
        var sw = Stopwatch.StartNew();
        var statusCode = "error";

        var target = MapTarget(request.RequestUri!.AbsolutePath);
        
        try
        {
            response = await base.SendAsync(request, cancellationToken);
            statusCode = ((int)response.StatusCode).ToString();
            return response;
        }
        finally
        {
            sw.Stop();

            var tags = new TagList
            {
                { "target", target},
                { "http.status_code", statusCode }
            };
            
            _durationHistogram.Record(sw.Elapsed.TotalMilliseconds, tags);
            _requestCounter.Add(1, tags);
        }
    }

    private static string MapTarget(string path)
    {
        if (path.Contains("angebote/fahrplan")) return "connections";
        if (path.Contains("angebote/teilstrecke")) return "increased";
        if( path.Contains("reiseloesung/fahrt")) return "route";
        if( path.Contains("reiseloesung/orte")) return "stations";

        throw new InvalidDataException($"Endpoint {path} currently not present in metrics");
    }
}