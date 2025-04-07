using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.Routes;

public interface IRouteQueryRepository
{
    public Task<RouteDto?> GetAsync(RouteId id);
}