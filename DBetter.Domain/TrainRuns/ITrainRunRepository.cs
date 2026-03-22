using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainRuns;

public interface ITrainRunRepository
{
    Task<TrainRun?> GetAsync(TrainRunId id);

    Task<TrainRun?> GetAsync(TrainRunIdentifier identifier);

    Task<List<TrainRun>> GetManyAsync(IEnumerable<TrainRunIdentifier> identifiers);
    
    void AddRange(IEnumerable<TrainRun> trainRuns);
    
    void Add(TrainRun trainRun);
}