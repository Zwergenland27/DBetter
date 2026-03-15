using DBetter.Domain.Abstractions;
using DBetter.Domain.Routes.Events;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Routes.Stops;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes;

public class Route : AggregateRoot<RouteId>
{
    private List<Stop> _stops;
    
    public TrainRunId TrainRunId { get; private set; }
    
    public IReadOnlyList<Stop> Stops => _stops.OrderBy(s => s.RouteIndex.Value).ToList().AsReadOnly();
    
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

    public static Route CreateEmpty(TrainRunId trainRunId)
    {
        return new Route(
            RouteId.CreateNew(),
            trainRunId,
            [],
            RouteSource.Departure,
            DateTime.UtcNow);
    }

    public static Route CreateFromRoute(TrainRunId trainRunId, List<StopSnapshot> stopSnapshots)
    {
        var stops = new List<Stop>();
        short stopIndex = 0;
        foreach (var stop in stopSnapshots)
        {
            stops.Add(new Stop(
                new StopId(stopIndex),
                stop.RouteIndex,
                stop.StationId,
                stop.DepartureTime,
                stop.ArrivalTime,
                stop.Demand,
                stop.Platform,
                stop.Attributes));
            stopIndex++;
        }
        
        return new Route(RouteId.CreateNew(),  trainRunId, stops, RouteSource.Connection, DateTime.UtcNow);
    }

    public void UpdateFromRoute(List<StopSnapshot> stopSnapshots)
    {
        var lastStop = Stops.LastOrDefault();
        var stopIndex = (short) 0;
        if (lastStop is not null)
        {
            stopIndex = (short)(lastStop.Id.Value + 1);
        }
        foreach (var stop in stopSnapshots)
        {
            var existingStop = _stops.FirstOrDefault(s => s.StationId == stop.StationId);
            if (existingStop is not null)
            {
                existingStop.Update(
                    stop.RouteIndex,
                    stop.DepartureTime,
                    stop.ArrivalTime,
                    stop.Demand,
                    stop.Platform,
                    stop.Attributes);
                continue;
            }
            
            _stops.Add(new Stop(
                new StopId(stopIndex),
                stop.RouteIndex,
                stop.StationId,
                stop.DepartureTime,
                stop.ArrivalTime,
                stop.Demand,
                stop.Platform,
                stop.Attributes));
            stopIndex++;
        }

        if (Source is RouteSource.Historical or RouteSource.Departure)
        {
            Source = RouteSource.Connection;
        }
        
        LastUpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateFromTrainRun(List<StopSnapshot> stopSnapshots)
    {
        var containsRealTimeInformation = stopSnapshots.Any(s => s.DepartureTime?.Real is not null || s.ArrivalTime?.Real is not null);
        
        var lastStop = Stops.LastOrDefault();
        var inPast = lastStop is not null && lastStop.ArrivalTime!.Planned < DateTime.UtcNow.AddHours(-1);
        
        var stopIndex = (short) 0;
        if (lastStop is not null)
        {
            stopIndex = (short)(lastStop.Id.Value + 1);
        }
        foreach (var stop in stopSnapshots)
        {
            var existingStop = _stops.FirstOrDefault(s => s.StationId == stop.StationId);
            if (existingStop is not null && (containsRealTimeInformation || !inPast))
            {
                existingStop.Update(
                    stop.RouteIndex, 
                    stop.DepartureTime,
                    stop.ArrivalTime,
                    stop.Demand,
                    stop.Platform,
                    stop.Attributes);
                continue;
            }
            
            //TODO: Handle correct route index, but it will be removed anyways
            _stops.Add(new Stop(
                new StopId(stopIndex),
                stop.RouteIndex,
                stop.StationId,
                stop.DepartureTime,
                stop.ArrivalTime,
                stop.Demand,
                stop.Platform,
                stop.Attributes));
            stopIndex++;
        }
        
        var arrivalTime = Stops.Last().ArrivalTime!;
        var latestArrivalTime = arrivalTime.Real ?? arrivalTime.Planned;
        if (arrivalTime.Real < arrivalTime.Planned)
        {
            latestArrivalTime = arrivalTime.Planned;
        }

        if (DateTime.UtcNow - latestArrivalTime < TimeSpan.FromHours(2))
        {
            RaiseDomainEvent(new DelayCheckScheduledEvent(TrainRunId, latestArrivalTime.Add(TimeSpan.FromHours(2))));
        }

        if (Source is RouteSource.Historical or RouteSource.Connection or RouteSource.Departure)
        {
            Source = RouteSource.TrainRun;
            RaiseDomainEvent(new RouteInitializedEvent(Id));
        }
        
        LastUpdatedAt = DateTime.UtcNow;
    }
}