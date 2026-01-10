using DBetter.Domain.Abstractions;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Routes.Events;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes;

/// <summary>
/// Complete route of a vehicle
/// </summary>
public class Route : AggregateRoot<RouteId>
{
    private List<PassengerInformation> _messages = [];
    
    public BahnJourneyId JourneyId { get; private init; }
    
    public IReadOnlyList<PassengerInformation> Messages => _messages.AsReadOnly();
    
    public CateringInformation Catering  { get; private set; }
    
    public BikeCarriageInformation BikeCarriage { get; private set; }
    
    public ServiceInformation ServiceInformation { get; private set; }
    
    private Route() : base(null!){}

    private Route(
        RouteId id,
        BahnJourneyId journeyId,
        List<PassengerInformation> messages,
        ServiceInformation serviceInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation) : base(id)
    {
        JourneyId = journeyId;
        _messages = messages;
        ServiceInformation = serviceInformation;
        Catering = cateringInformation;
        BikeCarriage = bikeCarriageInformation;
    }
    
    public static Route CreateFromSnapshot(TransportSegmentSnapshot snapshot)
    {
        var route = new Route(
            RouteId.CreateNew(),
            snapshot.JourneyId,
            snapshot.InformationMessages,
            snapshot.Composition.First(),
            snapshot.Catering,
            snapshot.BikeCarriage);

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

    public void Update(TransportSegmentSnapshot snapshot)
    {
        Catering = Catering.Update(snapshot.Catering);
        BikeCarriage = BikeCarriage.Update(snapshot.BikeCarriage);
        _messages.AddRange(snapshot.InformationMessages);
        _messages = _messages.Distinct().ToList();
    }

    public void Update(RouteSnapshot snapshot)
    {
        Catering = Catering.Update(snapshot.Catering);
        BikeCarriage = BikeCarriage.Update(snapshot.BikeCarriage);
        _messages.AddRange(snapshot.InformationMessages);
        _messages = _messages.Distinct().ToList();
    }

    public void UpdateServiceNumber(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }
}