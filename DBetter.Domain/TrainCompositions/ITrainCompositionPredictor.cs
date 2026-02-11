using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public interface ITrainCompositionPredictor
{
    Task<TrainComposition?> PredictAsync(TrainCirculationId trainCirculationId, OperatingDay operatingDay);
}