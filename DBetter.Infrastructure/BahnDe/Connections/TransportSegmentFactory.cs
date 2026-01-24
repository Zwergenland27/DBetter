using DBetter.Application.Requests.Snapshots;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class TransportSegmentFactory(VerbindungsAbschnitt abschnitt)
{
    public TransportSegmentSnapshot GetTransportSegment()
    {
        var journeyId = new BahnJourneyId(abschnitt.JourneyId!);

        StationName? destination = null;
        var givenDestination = abschnitt.Verkehrsmittel.Richtung;

        if (givenDestination is not null)
        {
            var stationNameResult = StationName.Create(givenDestination);
            if (!stationNameResult.HasFailed)
            {
                destination = stationNameResult.Value;
            }
        }
        
        var routeInformationFactory = new TransportSegmentInformationFactory(abschnitt);
        
        return new TransportSegmentSnapshot
        {
            JourneyId = journeyId,
            BikeCarriage = routeInformationFactory.ExtractBikeCarriageInformation(),
            Catering = routeInformationFactory.ExtractCateringInformation(),
            Demand = abschnitt.GetDemand(),
            Destination = destination,
            PassengerInformation = routeInformationFactory.ExtractPassengerInformation(),
            Composition = routeInformationFactory.ExtractComposition(),
            Stops = abschnitt.Halte.Select(GetStop).ToList()
        };
    }
    
    private StopSnapshot GetStop(Halt halt)
    { 
        var attributes = new StopAttributes
        {
            IsAdditional = halt.IsAdditional(),
            IsCancelled = halt.IsCancelled(),
            IsEntryOnly = halt.IsEntryOnly(),
            IsExitOnly = halt.IsExitOnly(),
            IsRequestStop = false
        };
        
        return new StopSnapshot
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