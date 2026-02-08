using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes;

public interface IRouteRepository
{
    void AddRange(IEnumerable<Route> routes);

    Task<Route?> GetAsync(TrainRunId trainRunId);
    
    Task<List<Route>> GetManyAsync(IEnumerable<TrainRunId> trainRunIds);
}