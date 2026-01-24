using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.Routes.Dtos;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;

namespace DBetter.Application.Routes.Queries.Get;

public class RouteResponseFactory(Route route, List<Station> stations)
{
    public RouteResponse MapToResponse(RouteDto dto)
    {
        return new RouteResponse
        {
            Id = route.Id.Value.ToString(),
            Operator = null,
            BikeCarriage = route.BikeCarriage.ToResponse(),
            Catering = route.Catering.ToResponse(),
            TransportCategory = route.ServiceInformation.ProductClass,
            ProductClass = route.ServiceInformation.ProductClass,
            Line = route.ServiceInformation.LineNumber?.Value,
            ServiceNumber = route.ServiceInformation.ServiceNumber?.Value,
            Stops = dto.Stops.Select(MapToResponse).ToList()
        };
    }
    
    private StopResponse MapToResponse(StopDto dto)
    {
        var station = stations.First(station => station.EvaNumber == dto.EvaNumber);
        var attributes = dto.Attributes;

        return new StopResponse
        {
            Id = station.Id.Value.ToString(),
            Ril100 = station.Ril100?.Value,
            ArrivalTime = dto.ArrivalTime?.ToResponse(),
            DepartureTime = dto.DepartureTime?.ToResponse(),
            Demand = dto.Demand.ToResponse(),
            IsAdditional = attributes.IsAdditional,
            IsCancelled = attributes.IsCancelled,
            IsEntryOnly = attributes.IsEntryOnly,
            IsExitOnly = attributes.IsExitOnly,
            IsRequestOnly = attributes.IsRequestStop,
            Name = station.Name.Value,
            Platform = dto.Platform?.ToResponse(),
            StopIndex = dto.RouteIndex.Value,
        };
    }
}