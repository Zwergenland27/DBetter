using DBetter.Application.Requests.Dtos;
using DBetter.Application.Routes.Dtos;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Routes;

public class RouteSnapshotFactory(Fahrt fahrt)
{
    public RouteDto ExtractSnapshot()
    {
        var routeInformationFactory = new RouteInformationFactory(fahrt);
        
        return new RouteDto
        {
            BikeCarriage = routeInformationFactory.ExtractBikeCarriageInformation(),
            Catering = routeInformationFactory.ExtractCateringInformation(),
            PassengerInformation = routeInformationFactory.ExtractPassengerInformation(),
            ServiceNumbers = routeInformationFactory.ExtractServiceNumbers(),
            Stops = fahrt.Halte.Select(GetStop).ToList()
        };
    }
    
    private StopDto GetStop(Halt halt)
    {
        var isRequestStop = false;
        var attributes = new StopAttributes
        {
            IsAdditional = halt.IsAdditional(),
            IsCancelled = halt.IsCancelled(),
            IsEntryOnly = halt.IsEntryOnly(),
            IsExitOnly = halt.IsExitOnly(),
            IsRequestStop = isRequestStop
        };
        
        return new StopDto
        {
            EvaNumber = EvaNumber.Create(halt.ExtId).Value,
            DepartureTime = halt.GetDepartureTime(),
            ArrivalTime = halt.GetArrivalTime(),
            Attributes = attributes,
            Demand = halt.GetDemand(),
            InfoId = halt.GetStationInfoId(),
            Name = halt.GetStationName(),
            Platform = halt.GetPlatform(),
            RouteIndex = halt.GetStopIndex()
        };
    }
}