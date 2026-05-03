using CleanDomainValidation.Domain;

namespace DBetter.Domain.MultipleUnitVehicleSeriesVariant;

public static class MultipleUnitVehicleSeriesVariantErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("MultipleUnitVehicleSeriesVariant.Id.Invalid", $"{value} is no valid guid.");
    }
}