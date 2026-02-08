using DBetter.Domain.Abstractions;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes.Stops;

public class Stop : Entity<StopId>
{
    public StopIndex RouteIndex { get; private set; }
    public StationId StationId { get; private set; }
    
    public TravelTime? DepartureTime { get; private set; }
    
    public TravelTime? ArrivalTime { get; private set; }
    
    public StopAttributes Attributes { get; private set; }
    
    private Stop() : base(null){}

    internal Stop(
        StopId id,
        StopIndex routeIndex,
        StationId stationId,
        TravelTime? departureTime,
        TravelTime? arrivalTime,
        StopAttributes attributes) : base(id)
    {
        RouteIndex = routeIndex;
        StationId = stationId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        Attributes = attributes;
    }

    internal void Update(
        StopIndex routeIndex,
        TravelTime? departureTime,
        TravelTime? arrivalTime,
        StopAttributes attributes)
    {
        RouteIndex = routeIndex;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
    }
}