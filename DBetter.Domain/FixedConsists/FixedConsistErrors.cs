using CleanDomainValidation.Domain;

namespace DBetter.Domain.FixedConsists;

public static class FixedConsistErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("FixedConsist.Id.Invalid", $"{value} is no valid guid.");
    }
}