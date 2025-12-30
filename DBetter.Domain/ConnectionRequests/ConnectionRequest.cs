using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests;

public class ConnectionRequest : AggregateRoot<ConnectionRequestId>
{
    private List<Passenger> _passengers;
    
    public UserId? OwnerId { get; private init; }
    
    public DateTime? DepartureTime { get; private set; }
    
    public DateTime? ArrivalTime { get; private set; }
    
    public IReadOnlyList<Passenger> Passengers => _passengers.AsReadOnly();

    public bool AllPassengersOwnDeutschlandTicket => Passengers.All(p => p.OwnsDeutschlandTicket);

    public bool AnyBikes => Passengers.Any(p => p.Bikes > 0);
    
    public ComfortClass ComfortClass { get; private set; }
    
    public Route Route { get; private set; }
    
    public PaginationReference? EarlierReference { get; private set; }
    
    public PaginationReference? LaterReference { get; private set; }

    private ConnectionRequest() : base(null!){}
    
    private ConnectionRequest(
        ConnectionRequestId id,
        UserId? ownerId,
        DateTime? departureTime,
        DateTime? arrivalTime,
        List<Passenger> passengers,
        ComfortClass comfortClass,
        Route route) : base(id)
    {
        OwnerId = ownerId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        _passengers = passengers;
        ComfortClass = comfortClass;
        Route = route;
    }

    public static CanFail<ConnectionRequest> Create(
        ConnectionRequestId id,
        UserId? ownerId,
        DateTime? departureTime,
        DateTime? arrivalTime,
        List<Passenger> passengers,
        ComfortClass comfortClass,
        Route route)
    {
        if (departureTime is null && arrivalTime is null) return DomainErrors.ConnectionRequest.NoTimeSpecified;
        return new ConnectionRequest(id, ownerId, departureTime, arrivalTime, passengers, comfortClass, route);
    }

    public void InitializeLaterReference(string earlierRef, string laterRef)
    {
        EarlierReference = PaginationReference.Create(earlierRef);
        LaterReference = PaginationReference.Create(laterRef);
    }

    public CanFail EarlierUsed(string earlierRef)
    {
        if (EarlierReference is null) return DomainErrors.ConnectionRequest.ReferencesNotInitialized;
        EarlierReference = PaginationReference.Create(earlierRef);
        return CanFail.Success;
    }
    
    public CanFail LaterUsed(string laterRef)
    {
        if (LaterReference is null) return DomainErrors.ConnectionRequest.ReferencesNotInitialized;
        LaterReference = PaginationReference.Create(laterRef);
        return CanFail.Success;
    }
}