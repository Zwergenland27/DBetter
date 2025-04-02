using DBetter.Domain.Abstractions;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes;

/// <summary>
/// Journey without any information
/// </summary>
/// <remarks>
/// Necessary to map journeyId to Bahn journey id
/// </remarks>
public class Route : AggregateRoot<RouteId>
{
    private readonly List<RoutePassengerInformation> _messages = [];
    
    private readonly List<Stop> _stops = [];
    
    public IReadOnlyList<RoutePassengerInformation> Messages => _messages.AsReadOnly();
    
    public CateringInformation Catering { get; private init; }
    
    public BikeCarriage BikeCarriage { get; private init; }
    
    public RouteInformation RouteInfos { get; private init; }
    
    public IReadOnlyList<Stop> Stops => _stops.AsReadOnly();
    
    private Route() : base(null!){}

    public Route(
        RouteId id,
        List<RoutePassengerInformation> messages,
        RouteInformation routeInfos,
        CateringInformation catering,
        BikeCarriage bikeCarriage,
        List<Stop> stops) : base(id)
    {
        _messages = messages;
        RouteInfos = routeInfos;
        Catering = catering;
        BikeCarriage = bikeCarriage;
        _stops = stops;
    }
}