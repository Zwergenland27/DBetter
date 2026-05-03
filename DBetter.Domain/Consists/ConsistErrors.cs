using CleanDomainValidation.Domain;

namespace DBetter.Domain.Consists;

public static class ConsistErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("Consist.Id.Invalid", $"{value} is no valid guid.");
    }
}