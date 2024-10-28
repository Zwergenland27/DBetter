using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Users.ValueObjects;

public record UserId(Guid Value)
{
    public static UserId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<UserId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new UserId(guid);
        }

        return DomainErrors.User.Id.Invalid;
    }
}