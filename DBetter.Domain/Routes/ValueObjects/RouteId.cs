using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Routes.ValueObjects;

public record RouteId(Guid Value)
{
    public static RouteId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<RouteId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new RouteId(guid);
        }

        return DomainErrors.Route.Id.Invalid(value);
    }
}