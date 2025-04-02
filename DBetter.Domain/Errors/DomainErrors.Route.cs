using CleanDomainValidation.Domain;
using DBetter.Domain.Route.ValueObjects;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Route
    {
        public static Error NotFound(RouteId id) => Error.NotFound("Route.NotFound",
            $"A route with the id {id.Value} was not found.");
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Route.Id.Invalid", $"{value} is no valid guid.");
        }
    }
}