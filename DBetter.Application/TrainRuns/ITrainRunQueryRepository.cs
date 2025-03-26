using DBetter.Domain.TrainRun;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Application.TrainRuns;

public interface ITrainRunQueryRepository
{
    public Task<TrainRun?> GetAsync(TrainRunId id);
}