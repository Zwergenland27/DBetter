using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCompositions;

public interface ITrainCompositionPredictor
{
    Task<PredictionResult?> PredictAsync(TrainCirculationId trainCirculationId, OperatingDay operatingDay);
}

public record PredictionResult(TrainComposition PredictedComposition, double Score, double Probability);