using DBetter.Domain.Abstractions;
using DBetter.Domain.Shared;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Domain.Journey;

/// <summary>
/// Journey without any information
/// </summary>
/// <remarks>
/// Necessary to map journeyId to Bahn journey id
/// </remarks>
public class TrainRun : AggregateRoot<TrainRunId>
{
    private readonly List<RoutePassengerInfo> _messages = [];
    
    private readonly List<Stop> _stops = [];
    
    public BahnJourneyId BahnId { get; private init; }
    
    public IReadOnlyList<RoutePassengerInfo> Messages => _messages.AsReadOnly();
    
    public CateringInformation Catering { get; private init; }
    
    public BikeCarriage BikeCarriage { get; private init; }
    
    public TrainInformation Train { get; private init; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    private TrainRun() : base(null!){}

    private TrainRun(
        TrainRunId id,
        BahnJourneyId bahnId,
        List<RoutePassengerInfo> messages,
        TrainInformation train,
        CateringInformation catering,
        BikeCarriage bikeCarriage,
        List<Stop> stops) : base(id)
    {
        BahnId = bahnId;
        _messages = messages;
        Train = train;
        Catering = catering;
        BikeCarriage = bikeCarriage;
        _stops = stops;
    }
}