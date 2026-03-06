using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public interface ITrainCompositionScraper
{
    Task ScheduleUpdate(TrainRunId trainRunId, DateTime scheduledDate);
}