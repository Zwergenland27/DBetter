using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

public interface ITrainRunRepository
{
    Task<TrainRun?> GetAsync(TrainRunId id);

    Task<TrainRun?> GetAsync(TrainRunIdentifier identifier);

    Task<List<TrainRun>> GetManyAsync(IEnumerable<TrainRunIdentifier> identifiers);
    
    void Save(IEnumerable<TrainRun> trainRuns);
    
    void Save(TrainRun trainRun);
}