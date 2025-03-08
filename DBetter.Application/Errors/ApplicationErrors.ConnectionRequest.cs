using CleanDomainValidation.Domain;

namespace DBetter.Application.Errors;

public static partial class ApplicationErrors
{
    public static class ConnectionRequest
    {
        public static class Put
        {
            public static class Id
            {
                public static Error Missing => Error.Validation(
                    "ConnectionRequest.Put.Id.Missing",
                    "The id is required");
            }

            public static class Passengers
            {
                public static Error Missing => Error.Validation(
                    "ConenctionRequest.Put.Passengers.Missing",
                    "The passengers list is required");

                public static class Id
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Passengers.Id.Missing",
                        "The id is required");
                }

                public static class Options
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Passengers.Options.Missing",
                        "The passenger options are required");

                    public static class Reservation
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.Passengers.Options.Reservation.Missing",
                            "The reservation field of passenger options is required");
                    }

                    public static class Bikes
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.Passengers.Options.Bikes.Missing",
                            "The bikes field of passenger options is required");
                    }

                    public static class Dogs
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.Passengers.Options.Dogs.Missing",
                            "The dogs field of passenger options is required");
                    }

                    public static class NeedsAccessibility
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.Passengers.Options.NeedsAccessibility.Missing",
                            "The needsAccesibility field of passenger options is required");
                    }
                }

                public static class Discounts
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Passengers.Discounts.Missing",
                        "The discounts list is required");

                    public static class Type
                    {
                        public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Passengers.Discounts.Type.Missing",
                        "The type of the discount is required");
                    }
                    
                    public static class Class
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.Passengers.Discounts.Class.Missing",
                            "The class of the discount is required");
                        
                        public static Error Invalid => Error.Validation(
                            "ConnectionRequest.Put.Passengers.Discounts.Class.Invalid", 
                            "The Discount type is invalid");
                    }
                }
            }

            public static class Options
            {
                public static Error Missing => Error.Validation(
                    "ConnectionRequest.Put.Options.Missing",
                    "The options field is required");

                public static class Class
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Options.Class.Missing",
                        "The class option is required");
                }

                public static class MaxTransfers
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.MaxTransfers.Missing",
                        "The max transfer option is required");
                }

                public static class MinTransferMinutes
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.MinTransferMinutes.Missing",
                        "The min transfer minutes option is required");
                }
            }

            public static class Route
            {
                public static Error Missing => Error.Validation(
                    "ConnectionRequest.Put.Route.Missing",
                    "The route is required");

                public static class Stops
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Route.Stops.Missing",
                        "The stop list is required");
                }

                public static class AllowedVehicles
                {
                    public static Error Missing => Error.Validation(
                        "ConnectionRequest.Put.Route.AllowedVehicles.Missing",
                        "The allowed vehicle list is required");

                    public static class HighSpeed
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.AllowedVehicles.HighSpeed.Missing",
                            "The high speed option is required");
                    }

                    public static class Intercity
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.AllowedVehicles.Intercity.Missing",
                            "The intercity option is required");
                    }

                    public static class Regional
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.AllowedVehicles.Regional.Missing",
                            "The regional option is required");
                    }

                    public static class PublicTransport
                    {
                        public static Error Missing => Error.Validation(
                            "ConnectionRequest.Put.AllowedVehicles.PublicTransport.Missing",
                            "The public transport option is required");
                    }
                }
            }
        }
    }
}