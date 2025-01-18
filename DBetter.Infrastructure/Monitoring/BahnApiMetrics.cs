using System.Diagnostics.Metrics;
using System.Net;

namespace DBetter.Infrastructure.Monitoring;

public class BahnApiMetrics
{
    public const string Metername = "DBetter.Api";

    private readonly Counter<long> _stationRequestCounter;

    private readonly Counter<long> _tripRequestCounter;

    private readonly Counter<long> _vehicleRequestCounter;

    public BahnApiMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(Metername);
        _stationRequestCounter = meter.CreateCounter<long>(
            "dbetter.api.station_requests.count");
        
        _tripRequestCounter = meter.CreateCounter<long>(
            "dbetter.api.trips_requests.count");
        
        _vehicleRequestCounter = meter.CreateCounter<long>(
            "dbetter.api.vehicle_requests.count");
    }

    public void IncreaseStationRequestCount(HttpStatusCode statusCode)
    {
        _stationRequestCounter.Add(1, new KeyValuePair<string, object?>("http_response_status_code", (int) statusCode));
    }

    public void IncreaseTripRequestCount(HttpStatusCode statusCode)
    {
        _tripRequestCounter.Add(1, new KeyValuePair<string, object?>("http_response_status_code", (int) statusCode));
    }

    public void IncreaseVehicleRequestCount(bool realtime, HttpStatusCode statusCode)
    {
        _vehicleRequestCounter.Add(1,
            new  KeyValuePair<string, object?>("realtime", realtime),
            new KeyValuePair<string, object?>("http_response_status_code", (int) statusCode));
    }
}