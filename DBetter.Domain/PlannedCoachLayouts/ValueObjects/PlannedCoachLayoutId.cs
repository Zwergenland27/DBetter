using CleanDomainValidation.Domain;

namespace DBetter.Domain.PlannedCoachLayouts.ValueObjects;

public record PlannedCoachLayoutId(Guid Value)
{
    public static PlannedCoachLayoutId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<PlannedCoachLayoutId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new PlannedCoachLayoutId(guid);
        }

        return PlannedCoachLayoutErrors.Id.Invalid(value);
    }
}