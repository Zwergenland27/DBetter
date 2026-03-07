using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

public interface IDelayCheckScraper
{
    Task ScheduleDelayCheck(TrainRunId trainRunId, DateTime scheduledDate);
}