using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Route
    {
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Route.Id.Invalid", $"{value} is no valid guid.");
        }
    }
}