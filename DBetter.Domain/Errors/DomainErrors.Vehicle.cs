using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Vehicle
    {
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Vehicle.Id.Invalid", $"{value} is no valid guid.");
        }
    }
}