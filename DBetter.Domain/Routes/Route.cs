using DBetter.Domain.Abstractions;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes.Entities;
using DBetter.Domain.Routes.Events;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes;

/// <summary>
/// Complete route of a vehicle
/// </summary>
public class Route : AggregateRoot<RouteId>
{
    private List<RoutePassengerInformation> _passengerInformation = [];
    
    public BahnJourneyId JourneyId { get; private init; }
    
    public IReadOnlyList<RoutePassengerInformation> PassengerInformation => _passengerInformation.AsReadOnly();
    
    public CateringInformation Catering  { get; private set; }
    
    public BikeCarriageInformation BikeCarriage { get; private set; }
    
    public ServiceInformation ServiceInformation { get; private set; }
    
    private Route() : base(null!){}

    private Route(
        RouteId id,
        BahnJourneyId journeyId,
        List<RoutePassengerInformation> passengerInformation,
        ServiceInformation serviceInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation) : base(id)
    {
        JourneyId = journeyId;
        _passengerInformation = passengerInformation;
        ServiceInformation = serviceInformation;
        Catering = cateringInformation;
        BikeCarriage = bikeCarriageInformation;
    }
    
    public static Route Create(
        BahnJourneyId journeyId,
        List<RoutePassengerInformationSnapshot> passengerInformation,
        ServiceInformation serviceInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation)
    {
        var route = new Route(
            RouteId.CreateNew(),
            journeyId,
            [],
            serviceInformation,
            cateringInformation,
            bikeCarriageInformation);

        route.ReconcilePassengerInformation(passengerInformation);
        
        if (route.ServiceInformation.TransportCategory is
            TransportCategory.HighSpeedTrain or
            TransportCategory.FastTrain or
            TransportCategory.RegionalTrain or
            TransportCategory.SuburbanTrain)
        {
            route.RaiseDomainEvent(new RouteScrapingScheduledEvent(route.Id));
        }
        return route;
    }

    public void Update(CateringInformation cateringInformation)
    {
        Catering = Catering.Update(cateringInformation);
    }
    
    public void Update(BikeCarriageInformation bikeCarriageInformation)
    {
        BikeCarriage = BikeCarriage.Update(bikeCarriageInformation);
    }

    public void ReconcilePassengerInformation(List<RoutePassengerInformationSnapshot> incomingPassengerInformation)
    {
        var remainingPassengerInformation = incomingPassengerInformation;
        foreach (var existingPassengerInformation in _passengerInformation)
        {
            var incoming = remainingPassengerInformation
                .FirstOrDefault(im => im.Id == existingPassengerInformation.InformationId);
            if (incoming is not null)
            {
                remainingPassengerInformation.Remove(incoming);
            }
        }

        foreach (var passengerInformation in remainingPassengerInformation)
        {
            var routePassengerInformation = RoutePassengerInformation.Create(passengerInformation.Id, passengerInformation.FromStopIndex, passengerInformation.ToStopIndex);
            _passengerInformation.Add(routePassengerInformation);
        }
    }
    
    public void Update(List<RoutePassengerInformation> passengerInformationIds)
    {
        _passengerInformation.AddRange(passengerInformationIds);
        _passengerInformation = _passengerInformation.Distinct().ToList();
    }

    public void Update(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }
}