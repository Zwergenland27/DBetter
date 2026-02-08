using DBetter.Domain.Abstractions;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Routes.Stops;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes;

public class Route : AggregateRoot<RouteId>
{
    private List<Stop> _stops;
    
    public TrainRunId TrainRunId { get; private set; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    public RouteSource Source { get; private set; }
    
    public DateTime LastUpdatedAt { get; private set; }
    
    private Route() : base(null!)
    {
    }

    private Route(
        RouteId id,
        TrainRunId trainRunId,
        List<Stop> stops,
        RouteSource source,
        DateTime lastUpdatedAt) : base(id)
    {
        TrainRunId = trainRunId;
        _stops = stops;
        Source = source;
        LastUpdatedAt = lastUpdatedAt;
    }

    public static Route CreateFromRoute(TrainRunId trainRunId, List<StopSnapshot> stopSnapshots)
    {
        var stops = new List<Stop>();
        short stopIndex = 0;
        foreach (var stop in stopSnapshots)
        {
            stops.Add(new Stop(new StopId(stopIndex), stop.RouteIndex, stop.StationId, stop.DepartureTime, stop.ArrivalTime, stop.Attributes));
            stopIndex++;
        }
        
        return new Route(RouteId.CreateNew(),  trainRunId, stops, RouteSource.Connection, DateTime.UtcNow);
    }

    public void UpdateFromRoute(List<StopSnapshot> stopSnapshots)
    {
        var stopIndex = (short) (_stops.OrderBy(s => s.Id).Last().Id.Value + 1);
        foreach (var stop in stopSnapshots)
        {
            var existingStop = _stops.FirstOrDefault(s => s.StationId == stop.StationId);
            if (existingStop is not null)
            {
                existingStop.Update(stop.RouteIndex, stop.DepartureTime, stop.ArrivalTime, stop.Attributes);
                continue;
            }
            
            _stops.Add(new Stop(new StopId(stopIndex), stop.RouteIndex, stop.StationId, stop.DepartureTime, stop.ArrivalTime, stop.Attributes));
            stopIndex++;
        }

        if (Source is RouteSource.Historical)
        {
            Source = RouteSource.Connection;
        }
        
        LastUpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateFromTrainRun(List<StopSnapshot> stopSnapshots)
    {
        var stopIndex = (short) (_stops.OrderBy(s => s.Id).Last().Id.Value + 1);
        foreach (var stop in stopSnapshots)
        {
            var existingStop = _stops.FirstOrDefault(s => s.StationId == stop.StationId);
            if (existingStop is not null)
            {
                existingStop.Update(stop.RouteIndex, stop.DepartureTime, stop.ArrivalTime, stop.Attributes);
                continue;
            }
            
            _stops.Add(new Stop(new StopId(stopIndex), stop.RouteIndex, stop.StationId, stop.DepartureTime, stop.ArrivalTime, stop.Attributes));
            stopIndex++;
        }

        if (Source is RouteSource.Historical or RouteSource.Connection)
        {
            Source = RouteSource.TrainRun;
        }
        
        LastUpdatedAt = DateTime.UtcNow;
    }
}