using CleanDomainValidation.Domain;

namespace DBetter.Domain.Vehicles;

public static class VehicleErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("Vehicle.Id.Invalid", $"{value} is no valid guid.");
    }
}