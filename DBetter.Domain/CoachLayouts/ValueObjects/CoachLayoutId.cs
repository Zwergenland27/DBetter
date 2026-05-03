using CleanDomainValidation.Domain;

namespace DBetter.Domain.CoachLayouts.ValueObjects;

public record CoachLayoutId(Guid Value)
{
    public static CoachLayoutId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<CoachLayoutId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new CoachLayoutId(guid);
        }

        return CoachLayoutErrors.Id.Invalid(value);
    }
}