using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Shared;

public record Stop
{
    private readonly List<PassengerInfo> _messages = [];
    
    public StationId Id { get; private init; }
    
    public StopIndex StopIndex { get; private init; }
    
    public Platform? Platform { get; private init; }
    
    public Demand Demand { get; private init; }
    
    public bool IsAdditional { get; private init; }
    
    public bool IsCancelled { get; private init; }
    
    public bool IsExitOnly { get; private init; }
    
    public bool IsEntryOnly { get; private init; }
    
    public DepartureTime? DepartureTime { get; private init; }
    
    public ArrivalTime? ArrivalTime { get; private init; }
    
    public IReadOnlyList<PassengerInfo> Messages => _messages.AsReadOnly();

    private Stop(){}
    
    public Stop(
        StationId id,
        StopIndex stopIndex,
        Platform? platform,
        Demand demand,
        bool isAdditional,
        bool isCancelled,
        bool isExitOnly,
        bool isEntryOnly,
        DepartureTime? departureTime,
        ArrivalTime? arrivalTime,
        List<PassengerInfo> messages)
    {
        Id = id;
        StopIndex = stopIndex;
        Platform = platform;
        Demand = demand;
        IsAdditional = isAdditional;
        IsCancelled = isCancelled;
        IsExitOnly = isExitOnly;
        IsEntryOnly = isEntryOnly;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        _messages = messages;
    }
}