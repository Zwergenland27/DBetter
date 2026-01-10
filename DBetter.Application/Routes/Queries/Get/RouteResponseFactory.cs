using DBetter.Application.Requests.GetSuggestions;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Stations;

namespace DBetter.Application.Routes.Queries.Get;

public class RouteResponseFactory(Route route, List<Station> stations)
{
    public RouteResponse MapToResponse(RouteSnapshot snapshot)
    {
        return new RouteResponse
        {
            RouteId = route.Id.Value.ToString(),
            Operator = null,
            BikeCarriage = route.BikeCarriage.ToResponse(),
            Catering = route.Catering.ToResponse(),
            TransportCategory = route.ServiceInformation.ProductClass,
            Line = route.ServiceInformation.LineNumber?.Value,
            ServiceNumber = route.ServiceInformation.ServiceNumber?.Value,
            Stops = snapshot.Stops.Select(MapToResponse).ToList()
        };
    }
    
    private StopResponse MapToResponse(StopSnapshot snapshot)
    {
        var station = stations.First(station => station.EvaNumber == snapshot.EvaNumber);
        var attributes = snapshot.Attributes;

        return new StopResponse
        {
            Id = station.Id.Value.ToString(),
            Ril100 = station.Ril100?.Value,
            ArrivalTime = snapshot.ArrivalTime?.ToResponse(),
            DepartureTime = snapshot.DepartureTime?.ToResponse(),
            Demand = snapshot.Demand.ToResponse(),
            IsAdditional = attributes.IsAdditional,
            IsCancelled = attributes.IsCancelled,
            IsEntryOnly = attributes.IsEntryOnly,
            IsExitOnly = attributes.IsExitOnly,
            IsRequestOnly = attributes.IsRequestStop,
            Name = station.Name.Value,
            Platform = snapshot.Platform?.ToResponse(),
            StopIndex = snapshot.RouteIndex.Value,
        };
    }
}