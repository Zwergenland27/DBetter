using CleanDomainValidation.Domain;

namespace DBetter.Domain.Consists.ValueObjects;

public record ConsistId(Guid Value)
{
    public static ConsistId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<ConsistId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new ConsistId(guid);
        }

        return ConsistErrors.Id.Invalid(value);
    }
}