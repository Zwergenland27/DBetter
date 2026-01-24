using System.Runtime.InteropServices.ComTypes;
using DBetter.Application.Requests.Snapshots;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.Connections;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;

namespace DBetter.Application.Requests.GetSuggestions;

public class ConnectionResponseFactory(
    List<Connection> connections, 
    List<Route> routes,
    List<Station> stations,
    List<PassengerInformation> passengerInformation)
{
    public ConnectionResponse MapToResponse(ConnectionSnapshot snapshot)
    {
        var segments = snapshot.Segments.Select(MapToResponse).ToList();
        var differentOrigin = false;
        var differentDestination = false;
        
        var firstSegment = segments.First();
        if (firstSegment is WalkingSegmentResponse)
        {
            differentOrigin = true;
            segments.Remove(firstSegment);
        }
        var lastSegment = segments.Last();
        if (lastSegment is WalkingSegmentResponse)
        {
            differentDestination = true;
            segments.Remove(lastSegment);
        }
        
        return new ConnectionResponse
        {
            Id = connections.First(c => c.ContextId == snapshot.ContextId).Id.Value.ToString(),
            DifferentOrigin = differentOrigin,
            DifferentDestination = differentDestination,
            Demand = snapshot.Demand.ToResponse(),
            BahnDeUrl = snapshot.BookingLink.Value,
            Offer = snapshot.Offer?.ToResponse(),
            Segments = segments
        };
    }

    private SegmentResponse MapToResponse(SegmentSnapshot snapshot)
    {
        return snapshot switch
        {
            TransferSegmentSnapshot transferSegmentSnapshot => MapToTransferSegmentResponse(transferSegmentSnapshot),
            TransportSegmentSnapshot transportSegmentSnapshot => MapToTransferSegmentResponse(transportSegmentSnapshot),
            WalkingSegmentSnapshot walkingSegmentSnapshot => MapToWalkingSegmentResponse(walkingSegmentSnapshot),
            _ => throw new ArgumentOutOfRangeException(nameof(snapshot))
        };
    }

    private WalkingSegmentResponse MapToWalkingSegmentResponse(WalkingSegmentSnapshot snapshot)
    {
        return new WalkingSegmentResponse
        {
            Distance = snapshot.Distance,
            WalkDuration = snapshot.WalkDuration
        };
    }

    private TransferSegmentResponse MapToTransferSegmentResponse(TransferSegmentSnapshot snapshot)
    {
        return new TransferSegmentResponse();
    }

    private TransportSegmentResponse MapToTransferSegmentResponse(TransportSegmentSnapshot snapshot)
    {
        var originStationEva = snapshot.JourneyId.OriginEvaNumber;
        var originStationName = stations.FirstOrDefault(station => station.EvaNumber == originStationEva)?.Name;
        var destinationStationEva = snapshot.JourneyId.DestinationEvaNumber;
        var destinationStationName =  stations.FirstOrDefault(station => station.EvaNumber == destinationStationEva)?.Name;
        
        var route = routes.First(route => route.JourneyId == snapshot.JourneyId);
        var serviceInformation = route.ServiceInformation;
        
        return new TransportSegmentResponse
        {
            RouteId = route.Id.Value.ToString(),
            DepartureTime = snapshot.Stops.First().DepartureTime!.ToResponse(),
            ArrivalTime = snapshot.Stops.Last().ArrivalTime!.ToResponse(),
            BikeCarriage = route.BikeCarriage.ToResponse(),
            Catering = route.Catering.ToResponse(),
            Demand = snapshot.Demand.ToResponse(),
            Origin = originStationName?.NormalizedValue,
            Destination = destinationStationName?.NormalizedValue ?? snapshot.Destination?.NormalizedValue,
            ProductClass = serviceInformation.ProductClass,
            Line = serviceInformation.LineNumber?.Value,
            ServiceNumber = serviceInformation.ServiceNumber?.Value,
            Operator = null,
            TransportCategory = serviceInformation.TransportCategory.ToString(),
            Messages = route.PassengerInformation
                .Select(pim => passengerInformation.First(pi => pi.Id == pim.InformationId).ToResponse())
                .ToList(),
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
            Name = station.Name.NormalizedValue,
            Platform = snapshot.Platform?.ToResponse(),
            StopIndex = snapshot.RouteIndex.Value,
        };
    }
}