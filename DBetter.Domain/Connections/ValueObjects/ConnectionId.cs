using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Connections.ValueObjects;

public record ConnectionId(Guid Value)
{
    public static ConnectionId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<ConnectionId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new ConnectionId(guid);
        }

        return DomainErrors.Connection.Id.Invalid(value);
    }
};