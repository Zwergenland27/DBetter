using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainCompositions;

public interface ITrainCompositionQueryRepository
{
    Task<TrainCompositionResultDto?> GetAsync(TrainRunId trainRunId);
    Task<List<TrainCompositionResultDto>?> GetManyAsync(IEnumerable<TrainRunId> trainRunIds);
}