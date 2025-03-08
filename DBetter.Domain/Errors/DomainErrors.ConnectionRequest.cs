using CleanDomainValidation.Domain;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class ConnectionRequest
    {
        public static Error NoTimeSpecified => Error.Conflict(
            "ConnectionRequest.NoTimeSpecified",
            "Either the departure or the arrival time must be set");
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("ConnectionRequest.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class Passenger
        {
            public static Error MissingAgeField(PassengerId passengerId) => Error.Conflict(
                "ConnectionRequest.Passenger.MissingAge",
                $"Either the birthday or the age of passenger {passengerId.Value} must be set");

            public static Error Max4Discounts(PassengerId passengerId) => Error.Conflict(
                "ConnectionRequest.Passenger.Max4Discounts",
                $"Passenger {passengerId.Value} cannot have more then 4 discounts");

            public static Error DuplicateDiscounts(PassengerId passengerId) => Error.Conflict(
                "ConnectionRequest.Passenger.DuplicateDiscounts",
                $"Passenger {passengerId.Value} cannot have a discount twice");
            public static class Id
            {
                public static Error Invalid(string value) => Error.Validation("ConnectionRequest.Passenger.Id.Invalid", $"{value} is no valid guid.");
            }
        }

        public static class Route
        {
            public static Error Min2Stops => Error.Conflict(
                "ConnectionRequest.Route.Min2Stops",
                "The route must contain at least 2 stops");
            public static Error Max2Stopovers => Error.Conflict(
                "ConnectionRequest.Route.Max2Stopovers",
                "The route cannot have more then 2 stopovers");

            public static Error AllowedVehiclesMismatch => Error.Conflict(
                "ConnectionRequest.Route.AllowedVehiclesMismatch",
                "The allowed vehicles must be defined for the section between each stop / stopover");

            public static Error NoVehicleAllowed => Error.Conflict(
                "ConnectionRequest.Route.NoVehicleAllowed",
                "At least one section does not allow any vehicles");
        }
    }
}