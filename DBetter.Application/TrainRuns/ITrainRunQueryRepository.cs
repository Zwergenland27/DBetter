using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.TrainRuns;

public interface ITrainRunQueryRepository
{
    public Task<Route?> GetAsync(RouteId id);
}