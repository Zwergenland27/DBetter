using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Routes.ValueObjects;

public record RoutePassengerInformationId(Guid Value)
{
    public static RoutePassengerInformationId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<RoutePassengerInformationId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new RoutePassengerInformationId(guid);
        }

        return DomainErrors.Route.PassengerInformation.Id.Invalid(value);
    }
}