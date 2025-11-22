using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;

namespace DBetter.Infrastructure.BahnDe.Routes.Entities;

public class RouteEntity
{
    private bool _hasBeenScraped = false;
    public RouteId Id { get; private set; }
    
    public BahnJourneyId BahnJourneyId { get; private set; }
    
    public ServiceInformation Information { get; private set; }
    
    /// <summary>
    /// Indicates that some information about the train run are missing and should be scraped
    /// </summary>
    public bool ScrapingRequired { get; private set; }

    #pragma warning disable CS8618
    /// <summary>
    /// Needed for EF Core
    /// </summary>
    private RouteEntity(){}
    #pragma warning restore CS8618

    public RouteEntity(
        RouteId id,
        BahnJourneyId bahnJourneyId,
        ServiceInformation information)
    {
        Id = id;
        BahnJourneyId = bahnJourneyId;
        Information = information;
    }

    public void UpdateServiceNumber(ServiceNumber serviceNumber)
    {
        Information = Information.UpdateServiceNumber(serviceNumber);
    }

    public void Scraped()
    {
        _hasBeenScraped = true;
        ScrapingRequired = false;
    }

    public void DestinationStationMissing()
    {
        if (_hasBeenScraped) return;
        
        ScrapingRequired = true;
    }
}