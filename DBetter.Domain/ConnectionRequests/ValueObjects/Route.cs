using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public class Route
{
    public StationId DepartureStationId  { get; private init; }
    
    public AllowedVehicles AllowedOnFirstSection { get; private init; }
    
    public Stopover? FirstStopOver { get; private init; }
    
    public AllowedVehicles? AllowedOnSecondSection { get; private init; }
    
    public Stopover? SecondStopOver { get; private init; }
    
    public AllowedVehicles? AllowedOnThirdSection { get; private init; }
    
    public StationId ArrivalStationId  { get; private init; }

    private Route(){}

    private Route(
        StationId departureStopId,
        AllowedVehicles allowedOnFirstSection,
        StationId arrivalStopId)
    {
        DepartureStationId = departureStopId;
        AllowedOnFirstSection = allowedOnFirstSection;
        ArrivalStationId = arrivalStopId;
    }
    
    private Route(
        StationId departureStopId,
        AllowedVehicles allowedOnFirstSection,
        Stopover firstStopOver,
        AllowedVehicles allowedOnSecondSection,
        StationId arrivalStopId)
    {
        DepartureStationId = departureStopId;
        AllowedOnFirstSection = allowedOnFirstSection;
        FirstStopOver = firstStopOver;
        AllowedOnSecondSection = allowedOnSecondSection;
        ArrivalStationId = arrivalStopId;
    }
    
    private Route(
        StationId departureStopId,
        AllowedVehicles allowedOnFirstSection,
        Stopover firstStopOver,
        AllowedVehicles allowedOnSecondSection,
        Stopover secondStopOver,
        AllowedVehicles allowedOnThirdSection,
        StationId arrivalStopId)
    {
        DepartureStationId = departureStopId;
        AllowedOnFirstSection = allowedOnFirstSection;
        FirstStopOver = firstStopOver;
        AllowedOnSecondSection = allowedOnSecondSection;
        SecondStopOver = secondStopOver;
        AllowedOnThirdSection = allowedOnThirdSection;
        ArrivalStationId = arrivalStopId;
    }

    public static CanFail<Route> Create(List<StationId> stopIds, List<AllowedVehicles> allowedVehicles)
    {
        if (stopIds.Count() < 2) return DomainErrors.ConnectionRequest.Route.Min2Stops;
        if (stopIds.Count() > 4) return DomainErrors.ConnectionRequest.Route.Max2Stopovers;
        if (allowedVehicles.Count() != stopIds.Count() - 1)
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

        if (stopIds.Count() == 2)
        {
            return new Route(
                stopIds[0],
                allowedVehicles[0],
                stopIds[1]);
        }

        if (stopIds.Count() == 3)
        {
            return new Route(
                stopIds[0], 
                allowedVehicles[0],
                new Stopover(stopIds[1], 0),
                allowedVehicles[1],
                stopIds[2]);
        }
        
        return new Route(
            stopIds[0], 
            allowedVehicles[0],
            new Stopover(stopIds[1], 0),
            allowedVehicles[1],
            new Stopover(stopIds[2], 0),
            allowedVehicles[2],
            stopIds[3]);
    }
}