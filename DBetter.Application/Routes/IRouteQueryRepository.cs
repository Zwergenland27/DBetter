using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.Routes;

public interface IRouteQueryRepository
{
    public Task<RouteResponse?> GetAsync(RouteId id);
}