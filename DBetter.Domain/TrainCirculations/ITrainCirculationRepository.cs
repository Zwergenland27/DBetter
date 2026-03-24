using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Domain.TrainCirculations;

public interface ITrainCirculationRepository
{
    Task<TrainCirculation?> GetAsync(TrainCirculationId id);
    Task<TrainCirculation?> GetAsync(TrainCirculationIdentifier identifier);
    Task<List<TrainCirculation>> GetManyAsync(IEnumerable<TrainCirculationIdentifier> identifiers);
    
    void Save(TrainCirculation trainCirculation);
    void Save(IEnumerable<TrainCirculation> trainCirculations);
}