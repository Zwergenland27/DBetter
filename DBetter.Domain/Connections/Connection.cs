using DBetter.Domain.Abstractions;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections;

public class Connection : AggregateRoot<ConnectionId>
{
    private readonly List<Message> _messages = [];

    private readonly List<Section> _sections = [];
    
    public TripId TripId { get; private set; }
    
    public ContextId ContextId { get; private set; }
    public Offer? Offer { get; private set; }
    
    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();
    
    public IReadOnlyList<Section> Sections => _sections.AsReadOnly();
    
    public Demand Demand { get; private set; }
    
    public bool? BikeCarriage { get; private set; }
    
    private Connection() : base(null!){}
    
    public Connection(
        ConnectionId id,
        TripId tripId,
        ContextId contextId,
        Offer? offer,
        List<Message> messages,
        Demand demand,
        bool? bikeCarriage,
        List<Section> sections) : base(id)
    {
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