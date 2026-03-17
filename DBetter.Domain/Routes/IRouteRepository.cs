using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Routes;

public interface IRouteRepository
{
    void AddRange(IEnumerable<Route> routes);
    
    void Add(Route route);

    Task<Route?> GetAsync(TrainRunId trainRunId);
    
    Task<Route?> GetAsync(RouteId routeId);
    
    Task<List<Route>> GetManyAsync(IEnumerable<TrainRunId> trainRunIds);
}