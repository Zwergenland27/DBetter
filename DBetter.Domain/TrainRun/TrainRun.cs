using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
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
    
    public TrainRunDate Date { get; private set; }
    
    public IReadOnlyList<RoutePassengerInfo> Messages => _messages.AsReadOnly();
    
    public CateringInformation Catering { get; private init; }
    
    public BikeCarriage BikeCarriage { get; private init; }
    
    public TrainInformation TrainInfos { get; private init; }
    
    public StationName? DestinationName { get; private set; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    private TrainRun() : base(null!){}

    private TrainRun(
        TrainRunId id,
        TrainRunDate date,
        List<RoutePassengerInfo> messages,
        TrainInformation trainInfos,
        CateringInformation catering,
        BikeCarriage bikeCarriage,
        List<Stop> stops,
        StationName? destinationName) : base(id)
    {
        _messages = messages;
        TrainInfos = trainInfos;
        Date = date;
        Catering = catering;
        BikeCarriage = bikeCarriage;
        _stops = stops;
        DestinationName = destinationName;
    }
}