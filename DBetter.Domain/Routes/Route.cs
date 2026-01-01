using DBetter.Domain.Abstractions;
using DBetter.Domain.Routes.Events;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes;

/// <summary>
/// Complete route of a vehicle
/// </summary>
public class Route : AggregateRoot<RouteId>
{
    private readonly List<PassengerInformation> _messages = [];
    
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

    public static Route CreateNew(
        BahnJourneyId journeyId,
        List<PassengerInformation> messages,
        ServiceInformation serviceInformation,
        CateringInformation cateringInformation,
        BikeCarriageInformation bikeCarriageInformation,
        bool stationsMissing)
    {
        var route = new Route(RouteId.CreateNew(), journeyId, messages, serviceInformation, cateringInformation,
            bikeCarriageInformation);

        if (stationsMissing)
        {
            route.RaiseDomainEvent(new RouteScrapingScheduledEvent(route.Id));   
        }
        
        return route;
    }

    public void UpdateServiceNumber(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }

    public void TryUpdateCateringInformation(CateringInformation newCateringInformation)
    {
        Catering = Catering.UpdateStopIndices(newCateringInformation.FromStopIndex, newCateringInformation.ToStopIndex);
    }
    
    public void TryUpdateBikeCarriageInformation(BikeCarriageInformation newBikeCarriageInformation)
    {
        BikeCarriage = BikeCarriage.UpdateStopIndices(newBikeCarriageInformation.FromStopIndex, newBikeCarriageInformation.ToStopIndex);
    }
}