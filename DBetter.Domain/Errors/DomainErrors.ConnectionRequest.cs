using CleanDomainValidation.Domain;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class ConnectionRequest
    {
        public static Error NoTimeSpecified => Error.Validation(
            "ConnectionRequest.NoTimeSpecified",
            "Either the departure or the arrival time must be set");
        
        public static Error NotFound => Error.NotFound(
            "ConnectionRequest.NotFound",
            "A connection request with the specified id was not found");
        
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("ConnectionRequest.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class Passenger
        {
            public static Error MissingAgeField(PassengerId passengerId) => Error.Validation(
                "ConnectionRequest.Passenger.MissingAge",
                $"Either the birthday or the age of passenger {passengerId.Value} must be set");

            public static Error Max4Discounts(PassengerId passengerId) => Error.Validation(
                "ConnectionRequest.Passenger.Max4Discounts",
                $"Passenger {passengerId.Value} cannot have more then 4 discounts");

            public static Error DuplicateDiscounts(PassengerId passengerId) => Error.Validation(
                "ConnectionRequest.Passenger.DuplicateDiscounts",
                $"Passenger {passengerId.Value} cannot have a discount twice");
            public static class Id
            {
                public static Error Invalid(string value) => Error.Validation("ConnectionRequest.Passenger.Id.Invalid", $"{value} is no valid guid.");
            }
        }

        public static class Route
        {
            public static Error FirstStopoverMissing => Error.Validation(
                "ConnectionRequest.Route.FirstStopoverMissing",
                "The second stopover is set without the first one");
            public static Error NoVehicleAllowed => Error.Validation(
                "ConnectionRequest.Route.NoVehicleAllowed",
                "At least one section does not allow any vehicles");

            public static class MaxTransfers
            {
                public static Error NegativeNotAllowed => Error.Validation(
                    "ConnectionRequest.Route.MaxTransfers.NegativeNotAllowed",
                    "A connection cannot contain a negative amount of transfers");
                public static Error Max10 => Error.Validation(
                    "ConnectionRequest.Route.MaxTransfers.Max10",
                    "A connection can contain a maximum of 10 transfers");
            }

            public static class MinTransferTime
            {
                public static Error NegativeNotAllowed => Error.Validation(
                    "ConnectionRequest.Route.MinTransferTime.NegativeNotAllowed",
                    "The transfer time cannot be negative");
                
                public static Error Max43 => Error.Validation(
                    "ConnectionRequest.Route.MinTransferTime.Max43",
                    "The transfer time cannot be more than 43 minutes");
            }
        }
    }
}