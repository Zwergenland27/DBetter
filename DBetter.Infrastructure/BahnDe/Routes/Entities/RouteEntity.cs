using CleanDomainValidation.Domain;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.TrainRuns.Entities;

public class RouteEntity
{
    private bool _hasBeenScraped = false;
    public RouteId Id { get; private set; }
    
    public JourneyId JourneyId { get; private set; }
    
    public RouteInformation RouteInfos { get; private set; }
    
    /// <summary>
    /// Indicates that some information about the train run are missing and should be scraped
    /// </summary>
    public bool ScrapingRequired { get; private set; }

    private RouteEntity(){}

    public RouteEntity(
        RouteId id,
        JourneyId journeyId,
        RouteInformation routeInfos)
    {
        Id = id;
        JourneyId = journeyId;
        RouteInfos = routeInfos;
    }

    public void UpdateTrainNumber(ServiceNumber serviceNumber)
    {
        RouteInfos = RouteInfos.UpdateServiceNumber(serviceNumber);
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