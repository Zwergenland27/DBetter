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
    
    public ComfortClass ComfortClass { get; private set; }
    
    public Route Route { get; private set; }

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
        UserId? ownerId,
        DateTime? departureTime,
        DateTime? arrivalTime,
        List<Passenger> passengers,
        ComfortClass comfortClass,
        Route route)
    {
        if (departureTime is null && arrivalTime is null) return DomainErrors.ConnectionRequest.NoTimeSpecified;
        return new ConnectionRequest(ConnectionRequestId.CreateNew(), ownerId, departureTime, arrivalTime, passengers, comfortClass, route);
    }
}