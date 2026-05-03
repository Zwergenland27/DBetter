using CleanDomainValidation.Domain;

namespace DBetter.Domain.FixedConsists.ValueObjects;

public record FixedConsistId(Guid Value)
{
    public static FixedConsistId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<FixedConsistId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new FixedConsistId(guid);
        }

        return FixedConsistErrors.Id.Invalid(value);
    }
}