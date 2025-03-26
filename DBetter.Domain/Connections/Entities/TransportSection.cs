using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

public class TransportSection : Section
{
    private readonly List<RoutePassengerInfo> _messages = [];
    
    private readonly List<TrainInformation> _trainParts = [];
    
    private readonly List<Stop> _stops = [];
    
    public Demand Demand { get; private set; }
    
    public IReadOnlyList<RoutePassengerInfo> Messages => _messages.AsReadOnly();
    
    public TrainRunId TrainRunId { get; private init; }
    
    public IReadOnlyList<TrainInformation> TrainParts => _trainParts.AsReadOnly();
    
    public StationName? Destination { get; private init; }
    
    public CateringInformation Catering { get; private init; }
    
    public BikeCarriage BikeCarriage { get; private init; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    private TransportSection(){}

    private TransportSection(
        SectionId id,
        Demand demand,
        List<RoutePassengerInfo> messages,
        TrainRunId trainRunId,
        List<TrainInformation> trainParts,
        StationName? destination,
        CateringInformation catering,
        BikeCarriage bikeCarriage,
        List<Stop> stops) : base(id)
    {
        Demand = demand;
        _messages = messages;
        TrainRunId = trainRunId;
        _trainParts = trainParts;
        Destination = destination;
        Catering = catering;
        BikeCarriage = bikeCarriage;
        _stops = stops;
    }

    public static TransportSection Create(
        Demand demand,
        List<RoutePassengerInfo> messages,
        TrainRunId trainRunId,
        List<TrainInformation> trains,
        StationName? destination,
        CateringInformation catering,
        BikeCarriage bikeCarriage,
        List<Stop> stops)
    {
        return new TransportSection(
            SectionId.CreateNew(),
            demand,
            messages,
            trainRunId,
            trains,
            destination,
            catering,
            bikeCarriage,
            stops);
    }
}