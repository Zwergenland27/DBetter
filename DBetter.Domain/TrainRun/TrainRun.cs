using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Domain.TrainRun;

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
    
    public IReadOnlyList<RoutePassengerInfo> Messages => _messages.AsReadOnly();
    
    public CateringInformation Catering { get; private init; }
    
    public BikeCarriage BikeCarriage { get; private init; }
    
    public TrainInformation TrainInfos { get; private init; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    private TrainRun() : base(null!){}

    public TrainRun(
        TrainRunId id,
        List<RoutePassengerInfo> messages,
        TrainInformation trainInfos,
        CateringInformation catering,
        BikeCarriage bikeCarriage,
        List<Stop> stops) : base(id)
    {
        _messages = messages;
        TrainInfos = trainInfos;
        Catering = catering;
        BikeCarriage = bikeCarriage;
        _stops = stops;
    }
}