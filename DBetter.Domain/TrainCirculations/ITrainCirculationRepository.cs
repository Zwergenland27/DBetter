using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Domain.TrainCirculations;

public interface ITrainCirculationRepository
{
    Task<TrainCirculation?> GetAsync(TrainCirculationId id);
    Task<List<TrainCirculation>> GetManyAsync(IEnumerable<TimeTableCompositeIdentifier> timeTableIdentifier);
    
    void AddRange(IEnumerable<TrainCirculation> trainCirculations);
}