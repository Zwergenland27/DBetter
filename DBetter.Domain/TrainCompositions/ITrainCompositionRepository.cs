using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public interface ITrainCompositionRepository
{
    void Add(TrainComposition trainComposition);
    
    Task<TrainComposition?> GetAsync(TrainRunId trainRunId);
    
    Task<List<TrainComposition>> GetAsync(IEnumerable<TrainRunId> trainRunIds);
}