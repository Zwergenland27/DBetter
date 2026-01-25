using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

public interface ITrainRunRepository
{
    Task<TrainRun?> GetAsync(TrainRunId id);

    Task<List<TrainRun>> GetManyAsync(IEnumerable<BahnJourneyId> jouneyIds);
    
    void AddRange(IEnumerable<TrainRun> trainRuns);
}