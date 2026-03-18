using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.Dtos;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class TransportSegmentFactory(VerbindungsAbschnitt abschnitt)
{
    public TransportSegmentDto GetTransportSegment()
    {
        var journeyId = BahnJourneyId.Create(abschnitt.JourneyId!);

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
        
        return new TransportSegmentDto
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
    
    private StopDto GetStop(Halt halt)
    { 
        var attributes = new StopAttributes
        {
            IsAdditional = halt.IsAdditional(),
            IsCancelled = halt.IsCancelled(),
            IsEntryOnly = halt.IsEntryOnly(),
            IsExitOnly = halt.IsExitOnly(),
            IsRequestStop = false
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