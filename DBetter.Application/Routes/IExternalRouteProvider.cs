using DBetter.Application.Routes.Dtos;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.Routes;

public interface IExternalRouteProvider
{
    Task<RouteSnapshot> GetRouteAsync(BahnJourneyId journeyId);
}