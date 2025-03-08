using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record ConnectionRequestId(Guid Value)
{
    public static ConnectionRequestId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<ConnectionRequestId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new ConnectionRequestId(guid);
        }

        return DomainErrors.ConnectionRequest.Id.Invalid(value);
    }
}