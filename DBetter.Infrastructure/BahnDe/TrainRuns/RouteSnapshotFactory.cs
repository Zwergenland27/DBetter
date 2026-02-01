using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Application.TrainRuns.Dtos;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.BahnDe.TrainRuns;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

namespace DBetter.Infrastructure.BahnDe.Routes;

public class RouteSnapshotFactory(Fahrt fahrt)
{
    public TrainRunDto ExtractSnapshot()
    {
        var routeInformationFactory = new TrainRunInformationFactory(fahrt);
        
        return new TrainRunDto
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
            TrainRunIndex = halt.GetStopIndex()
        };
    }
}