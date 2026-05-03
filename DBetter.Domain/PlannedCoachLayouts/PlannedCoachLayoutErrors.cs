using CleanDomainValidation.Domain;

namespace DBetter.Domain.PlannedCoachLayouts;

public static class PlannedCoachLayoutErrors
{
    public static class Id
    {
        public static Error Invalid(string value) => Error.Validation("PlannedCoachLayout.Id.Invalid", $"{value} is no valid guid.");
    }
}