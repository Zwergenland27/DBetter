using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record PassengerId(Guid Value)
{
    public static PassengerId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<PassengerId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new PassengerId(guid);
        }

        return DomainErrors.ConnectionRequest.Passenger.Id.Invalid(value);
    }
}