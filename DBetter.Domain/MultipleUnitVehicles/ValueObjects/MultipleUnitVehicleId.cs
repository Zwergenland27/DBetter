using CleanDomainValidation.Domain;

namespace DBetter.Domain.MultipleUnitVehicles.ValueObjects;

public record MultipleUnitVehicleId(Guid Value)
{
    public static MultipleUnitVehicleId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<MultipleUnitVehicleId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new MultipleUnitVehicleId(guid);
        }

        return MultipleUnitVehicleErrors.Id.Invalid(value);
    }
}