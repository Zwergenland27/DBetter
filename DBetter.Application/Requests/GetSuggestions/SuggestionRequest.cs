using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Shared;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionRequest
{
    public required List<SuggestionRequestPassenger> Passengers { get; init; }
    public required DateTime? DepartureTime { get; init; }
    public required DateTime? ArrivalTime { get; init; }
    public required ComfortClass ComfortClass { get; init; }
    public required SuggestionRequestRoute Route { get; init; }
    public required string? PaginationToken { get; init; }

    public required bool FastConnectionsOnly { get; init; }
    
    public required bool SeatReservationOnly { get; init; }
    
    public required bool DeutschlandTicketConnectionsOnly { get; init; }
    
    public bool AllPassengersOwnDeutschlandTicket => Passengers.Count > 0 && Passengers.All(p => p.OwnsDeutschlandTicket);

    public bool AnyBikes => Passengers.Any(p => p.Bikes > 0);

    public bool AnyDogs => Passengers.Any(p => p.Dogs > 0);
}