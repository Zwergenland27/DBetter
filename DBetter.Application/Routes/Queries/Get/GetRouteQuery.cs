using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanMediator.Commands;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Application.Routes.Queries.Get;

public class GetRouteQueryBuilder : IRequestBuilder<GetRouteParameters, GetRouteQuery>
{
    public ValidatedRequiredProperty<GetRouteQuery> Configure(RequiredPropertyBuilder<GetRouteParameters, GetRouteQuery> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, RouteId.Create);
        
        return builder.Build(() => new GetRouteQuery(id));
    }
}

public record GetRouteQuery(RouteId Id) : ICommand<RouteDto>;