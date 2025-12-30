using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class PartialStopIndexFactory
{
    public StopIndex From { get; private init; }
    
    public StopIndex To { get; private init; }
    
    public PartialStopIndexFactory(List<IRouteStop> stops, string? partialInformation)
    {
        var firstStationIndex = stops[0].RouteIdx;
        var lastStationIndex = stops[^1].RouteIdx;
        
        if (partialInformation is not null)
        {
            var bracesRemoved = partialInformation.Substring(1, partialInformation.Length - 2);
            var stations = bracesRemoved.Split(" - ");
        
            firstStationIndex = stops.First(stop => stop.Name == stations[0]).RouteIdx;
            lastStationIndex = stops.First(stop => stop.Name == stations[1]).RouteIdx;   
        }
        
        From = new StopIndex(firstStationIndex);
        To = new  StopIndex(lastStationIndex);
    }
}