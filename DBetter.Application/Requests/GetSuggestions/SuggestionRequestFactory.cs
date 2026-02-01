using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.IncreaseTransferTime;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations;

namespace DBetter.Application.Requests.GetSuggestions;

public class SuggestionRequestFactory(
    ConnectionRequest connectionRequest,
    List<Station> requestedStations)
{
    public SuggestionRequest Build(SuggestionMode suggestionMode = SuggestionMode.Normal)
    {
        return new SuggestionRequest
        {
            ArrivalTime = connectionRequest.ArrivalTime,
            DepartureTime = connectionRequest.DepartureTime,
            ComfortClass = connectionRequest.ComfortClass,
            DeutschlandTicketConnectionsOnly = false,
            FastConnectionsOnly = false,
            SeatReservationOnly = false,
            PaginationToken = connectionRequest.GetReferenceForMode(suggestionMode)?.Token,
            Passengers = connectionRequest.Passengers.Select(MapPassenger).ToList(),
            Route = MapRoute()
        };
    }
    
    private SuggestionRequestPassenger MapPassenger(Passenger passenger)
    {
        return new SuggestionRequestPassenger
        {
            Age = passenger.CalculateAge(),
            Bikes = passenger.Bikes,
            Dogs = passenger.Dogs,
            Discounts = passenger.Discounts.ToList()
        };
    }

    private SuggestionRequestRoute MapRoute()
    {
        var route = connectionRequest.Route;
        return new SuggestionRequestRoute
        {
            Origin = requestedStations.First(station => station.Id == route.OriginStationId),
            MeansOfTransportFirstSection = route.MeansOfTransportFirstSection,
            FirstStopover = MapStopover(route.FirstStopover),
            SecondStopover = MapStopover(route.SecondStopover),
            Destination = requestedStations.First(station => station.Id == route.DestinationStationId),
            MaxTransfers = route.MaxTransfers,
            MinTransferTime = route.MinTransferTime
        };
    }

    private SuggestionRequestStopover? MapStopover(Stopover? stopover)
    {
        if (stopover is null) return null;

        return new SuggestionRequestStopover()
        {
            Station = requestedStations.First(station => station.Id == stopover.StationId),
            StayDuration = stopover.LengthOfStay,
            MeansOfTransportNextSection = stopover.MeansOfTransportNextSection
        };
    }
}