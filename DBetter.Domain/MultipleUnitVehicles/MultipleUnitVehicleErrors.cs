using CleanDomainValidation.Domain;

namespace DBetter.Domain.MultipleUnitVehicles;

public static class MultipleUnitVehicleErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("MultipleUnitVehicle.Id.Invalid", $"{value} is no valid guid.");
    }
}