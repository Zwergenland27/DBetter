using CleanDomainValidation.Domain;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Connections.Entities;

public class TrainRunEntity
{
    public TrainRunId Id { get; private set; }
    
    public BahnJourneyId BahnId { get; private set; }
    
    public TrainRunDate Date { get; private set; }
    
    public TrainInformation TrainInfos { get; private set; }
    
    public StationName? DestinationName { get; private set; }

    private TrainRunEntity(){}

    private TrainRunEntity(
        TrainRunId id,
        BahnJourneyId bahnId,
        TrainRunDate date,
        TrainInformation trainInfos,
        StationName? destinationName)
    {
        Id = id;
        BahnId = bahnId;
        Date = date;
        TrainInfos = trainInfos;
        DestinationName = destinationName;
    }

    public static CanFail<TrainRunEntity> Create(
        TrainRunId id,
        BahnJourneyId bahnId,
        TrainInformation trainInfos,
        StationName? destinationName)
    {
        var dateResult = TrainRunDate.ExtractFromJourneyId(bahnId);
        if(dateResult.HasFailed) return dateResult.Errors;
        
        return new TrainRunEntity(id, bahnId, dateResult.Value, trainInfos, destinationName);
    }

    public void UpdateDestination(StationName? destinationName)
    {
        DestinationName = destinationName;
    }
}