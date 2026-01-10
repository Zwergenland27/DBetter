using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.Routes;

public interface IRouteRepository
{
    Task<Route?> GetAsync(RouteId id);

    Task<List<Route>> GetManyAsync(IEnumerable<BahnJourneyId> journeyIds);
    
    void AddRange(IEnumerable<Route> routes);
}