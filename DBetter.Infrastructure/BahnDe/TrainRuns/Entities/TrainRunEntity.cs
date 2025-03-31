using CleanDomainValidation.Domain;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.TrainRuns.Entities;

public class TrainRunEntity
{
    private bool _hasBeenScraped = false;
    public TrainRunId Id { get; private set; }
    
    public JourneyId JourneyId { get; private set; }
    
    public TrainInformation TrainInfos { get; private set; }
    
    /// <summary>
    /// Indicates that some information about the train run are missing and should be scraped
    /// </summary>
    public bool ScrapingRequired { get; private set; }

    private TrainRunEntity(){}

    public TrainRunEntity(
        TrainRunId id,
        JourneyId journeyId,
        TrainInformation trainInfos)
    {
        Id = id;
        JourneyId = journeyId;
        TrainInfos = trainInfos;
    }

    public void UpdateTrainNumber(TrainNumber trainNumber)
    {
        TrainInfos = TrainInfos.UpdateTrainNumber(trainNumber);
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