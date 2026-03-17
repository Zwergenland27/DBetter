using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Domain.TrainCirculations;

public interface ITrainCirculationRepository
{
    Task<TrainCirculation?> GetAsync(TrainCirculationId id);
    Task<TrainCirculation?> GetAsync(TimeTableCompositeIdentifier identifier);
    Task<List<TrainCirculation>> GetManyAsync(IEnumerable<TimeTableCompositeIdentifier> timeTableIdentifier);
    
    void Save(TrainCirculation trainCirculation);
    void Save(IEnumerable<TrainCirculation> trainCirculations);
}