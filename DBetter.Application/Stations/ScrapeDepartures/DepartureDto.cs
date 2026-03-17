using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Application.Stations.ScrapeDepartures;

public record DepartureDto
{
    public required BahnJourneyId JourneyId { get; init; }
    
    public required ServiceInformation ServiceInformation { get; init; }
}