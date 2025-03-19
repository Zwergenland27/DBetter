using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;

namespace DBetter.Domain.Connections;

public class Connection : AggregateRoot<ConnectionId>
{
    private readonly List<PassengerInfo> _messages = [];

    private readonly List<Section> _sections = [];
    
    public ConnectionRequestId RequestId { get; private set; }
    
    public TripId TripId { get; private set; }
    
    public ContextId ContextId { get; private set; }
    public Offer? Offer { get; private set; }
    
    public IReadOnlyList<PassengerInfo> Messages => _messages.AsReadOnly();
    
    public IReadOnlyList<Section> Sections => _sections.AsReadOnly();
    
    public Demand Demand { get; private set; }
    
    public bool? BikeCarriage { get; private set; }
    
    private Connection() : base(null!){}
    
    public Connection(
        ConnectionId id,
        ConnectionRequestId requestId,
        TripId tripId,
        ContextId contextId,
        Offer? offer,
        List<PassengerInfo> messages,
        Demand demand,
        bool? bikeCarriage,
        List<Section> sections) : base(id)
    {
        RequestId = requestId;
        TripId = tripId;
        ContextId = contextId;
        Offer = offer;
        _messages = messages;
        Demand = demand;
        BikeCarriage = bikeCarriage;
        _sections = sections;
    }

    public void BookOnBahnDe()
    {
        
    }

    public void ArriveEarlier()
    {
        
    }

    public void DepartLater()
    {
        
    }
}