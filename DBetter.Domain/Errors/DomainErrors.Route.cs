using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Route
    {
        public static Error NotFound =>
            Error.NotFound("Route.NotFound", "The route for this train has not been scraped.");
        
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Route.Id.Invalid", $"{value} is no valid guid.");
        }
    }
}