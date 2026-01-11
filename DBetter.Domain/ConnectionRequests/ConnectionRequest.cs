using CleanDomainValidation.Application.Lists;
using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.Events;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests;

public class ConnectionRequest : AggregateRoot<ConnectionRequestId>
{
    private List<Passenger> _passengers = [];
    private List<ConnectionId> _suggestedConnectionIds = [];
    
    public UserId? OwnerId { get; private init; }
    
    public DateTime? DepartureTime { get; private set; }
    
    public DateTime? ArrivalTime { get; private set; }
    
    public IReadOnlyList<Passenger> Passengers => _passengers.AsReadOnly();
    
    public ComfortClass ComfortClass { get; private set; }
    
    public Route Route { get; private set; }
    
    public PaginationReference? EarlierReference { get; private set; }
    
    public PaginationReference? LaterReference { get; private set; }
    
    public IReadOnlyList<ConnectionId> SuggestedConnectionIds => _suggestedConnectionIds.AsReadOnly();

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

    public CanFail UpdateReferences(SuggestionMode suggestionMode, PaginationReference? earlierRef, PaginationReference? laterRef)
    {
        if (suggestionMode is SuggestionMode.Normal)
        {
            foreach(var connectionId in _suggestedConnectionIds)
            {
                RaiseDomainEvent(new ConnectionContextFlushedEvent(connectionId));
            }
            _suggestedConnectionIds.Clear();
        }

        if (suggestionMode is SuggestionMode.Normal && earlierRef is not null && laterRef is not null)
        {
            EarlierReference = earlierRef;
            LaterReference = laterRef;
        }
        else if (suggestionMode is SuggestionMode.Earlier && earlierRef is not null)
        {
            if (EarlierReference is null) return DomainErrors.ConnectionRequest.ReferencesNotInitialized;
            EarlierReference = earlierRef;
        }else if (suggestionMode is SuggestionMode.Later && laterRef is not null)
        {
            if (LaterReference is null) return DomainErrors.ConnectionRequest.ReferencesNotInitialized;
            LaterReference = laterRef;
        }
        
        return CanFail.Success;
    }
    
    public PaginationReference? GetReferenceForMode(SuggestionMode suggestionMode)
    {
        return suggestionMode switch
        {
            SuggestionMode.Normal => null,
            SuggestionMode.Earlier => EarlierReference,
            SuggestionMode.Later => LaterReference,
            _ => throw new ArgumentOutOfRangeException(nameof(suggestionMode), suggestionMode, null)
        };
    }
    
    public void AddSuggestedConnections(IEnumerable<Connection> connections)
    {
        _suggestedConnectionIds.AddRange(connections.Select(c => c.Id));
        _suggestedConnectionIds = _suggestedConnectionIds.Distinct().ToList();
    }

    public void Update(
        DateTime? departureTime,
        DateTime? arrivalTime,
        List<Passenger> passengers,
        ComfortClass comfortClass,
        Route route)
    {
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        _passengers = passengers;
        ComfortClass = comfortClass;
        Route = route;
        
        foreach(var connectionId in _suggestedConnectionIds)
        {
            RaiseDomainEvent(new ConnectionContextFlushedEvent(connectionId));
        }
        _suggestedConnectionIds.Clear();
    }
}