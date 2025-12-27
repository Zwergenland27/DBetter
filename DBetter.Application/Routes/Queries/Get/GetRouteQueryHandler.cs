using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Domain.Errors;

namespace DBetter.Application.Routes.Queries.Get;

public class GetRouteQueryHandler(
    IRouteQueryRepository repository) : CommandHandlerBase<GetRouteQuery, RouteDto>
{
    public override async Task<CanFail<RouteDto>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        var route = await repository.GetAsync(request.Id);

        if(route is null) return DomainErrors.Route.NotFound(request.Id);
        
        return route;
    }
}