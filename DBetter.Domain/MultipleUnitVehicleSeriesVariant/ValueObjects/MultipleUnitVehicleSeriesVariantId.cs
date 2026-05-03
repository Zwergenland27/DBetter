using CleanDomainValidation.Domain;

namespace DBetter.Domain.MultipleUnitVehicleSeriesVariant.ValueObjects;

public record MultipleUnitVehicleSeriesVariantId(Guid Value)
{
    public static MultipleUnitVehicleSeriesVariantId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<MultipleUnitVehicleSeriesVariantId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new MultipleUnitVehicleSeriesVariantId(guid);
        }

        return MultipleUnitVehicleSeriesVariantErrors.Id.Invalid(value);
    }
}