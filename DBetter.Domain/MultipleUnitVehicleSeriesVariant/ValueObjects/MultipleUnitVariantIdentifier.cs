namespace DBetter.Domain.MultipleUnitVehicleSeriesVariant.ValueObjects;

public record MultipleUnitVariantIdentifier(string FamilyIdentifier, string VariantIdentifier)
{
    public static MultipleUnitVariantIdentifier Default(string familyIdentifier) => new MultipleUnitVariantIdentifier(familyIdentifier, string.Empty);
}