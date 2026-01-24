using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

public interface ITrainRunRepository
{
    Task<TrainRun?> GetAsync(TrainRunId id);

    Task<List<TrainRun>> GetManyAsync(IEnumerable<BahnJourneyId> journeyIds);
    
    void AddRange(IEnumerable<TrainRun> routes);
}