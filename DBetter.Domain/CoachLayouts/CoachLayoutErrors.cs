using CleanDomainValidation.Domain;

namespace DBetter.Domain.CoachLayouts;

public static class CoachLayoutErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("CoachLayout.Id.Invalid", $"{value} is no valid guid.");
    }
}