using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Routes;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;

namespace DBetter.Application.Routes.Queries.Get;

public class GetRouteQueryHandler(
    IRouteQueryRepository repository) : ICommandHandler<GetRouteQuery, RouteDto>
{
    public async Task<CanFail<RouteDto>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        var route = await repository.GetAsync(request.Id);

        if(route is null) return DomainErrors.Route.NotFound(request.Id);
        
        return route;
    }
}