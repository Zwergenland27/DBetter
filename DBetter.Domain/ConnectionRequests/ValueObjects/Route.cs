using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public class Route
{
    public EvaNumber DepartureStop  { get; private init; }
    
    public AllowedVehicles AllowedOnFirstSection { get; private init; }
    
    public Stopover? FirstStopOver { get; private init; }
    
    public AllowedVehicles? AllowedOnSecondSection { get; private init; }
    
    public Stopover? SecondStopOver { get; private init; }
    
    public AllowedVehicles? AllowedOnThirdSection { get; private init; }
    
    public EvaNumber ArrivalStop  { get; private init; }

    private Route(){}

    private Route(
        EvaNumber departureStop,
        AllowedVehicles allowedOnFirstSection,
        EvaNumber arrivalStop)
    {
        DepartureStop = departureStop;
        AllowedOnFirstSection = allowedOnFirstSection;
        ArrivalStop = arrivalStop;
    }
    
    private Route(
        EvaNumber departureStop,
        AllowedVehicles allowedOnFirstSection,
        Stopover firstStopOver,
        AllowedVehicles allowedOnSecondSection,
        EvaNumber arrivalStop)
    {
        DepartureStop = departureStop;
        AllowedOnFirstSection = allowedOnFirstSection;
        FirstStopOver = firstStopOver;
        AllowedOnSecondSection = allowedOnSecondSection;
        ArrivalStop = arrivalStop;
    }
    
    private Route(
        EvaNumber departureStop,
        AllowedVehicles allowedOnFirstSection,
        Stopover firstStopOver,
        AllowedVehicles allowedOnSecondSection,
        Stopover secondStopOver,
        AllowedVehicles allowedOnThirdSection,
        EvaNumber arrivalStop)
    {
        DepartureStop = departureStop;
        AllowedOnFirstSection = allowedOnFirstSection;
        FirstStopOver = firstStopOver;
        AllowedOnSecondSection = allowedOnSecondSection;
        SecondStopOver = secondStopOver;
        AllowedOnThirdSection = allowedOnThirdSection;
        ArrivalStop = arrivalStop;
    }

    public static CanFail<Route> Create(List<EvaNumber> stops, List<AllowedVehicles> allowedVehicles)
    {
        if (stops.Count() < 2) return DomainErrors.ConnectionRequest.Route.Min2Stops;
        if (stops.Count() > 4) return DomainErrors.ConnectionRequest.Route.Max2Stopovers;
        if (allowedVehicles.Count() != stops.Count() - 1)
            return DomainErrors.ConnectionRequest.Route.AllowedVehiclesMismatch;
        if (allowedVehicles.Any(allowed => allowed is
            {
                HighSpeed: false,
                Intercity: false,
                Regional: false,
                PublicTransport: false
            }))
        {
            return DomainErrors.ConnectionRequest.Route.NoVehicleAllowed;   
        }

        if (stops.Count() == 2)
        {
            return new Route(
                stops[0],
                allowedVehicles[0],
                stops[1]);
        }

        if (stops.Count() == 3)
        {
            return new Route(
                stops[0], 
                allowedVehicles[0],
                new Stopover(stops[1], 0),
                allowedVehicles[1],
                stops[2]);
        }
        
        return new Route(
            stops[0], 
            allowedVehicles[0],
            new Stopover(stops[1], 0),
            allowedVehicles[1],
            new Stopover(stops[2], 0),
            allowedVehicles[2],
            stops[3]);
    }
}