using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Station
    {
        public static Error NotFound => Error.NotFound("Station.NotFound", "A station with the specified id was not found.");
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Station.Id.Invalid", $"{value} is no valid guid.");
        }
        public static class EvaNumber
        {
            public static Error NotNumeric(string value) => Error.Validation(
                "Station.EvaNumber.Invalid", 
                $"{value} is not a numeric value.");
        }
        
        public static class InfoId
        {
            public static Error NotNumeric(string value) => Error.Validation(
                "Station.InfoId.Invalid", 
                $"{value} is not a numeric value.");
        }
        public static class Name
        {
            public static Error MetaStation(string value) => Error.Conflict(
                "Station.Name.MetaStation", 
                $"{value} is a meta station that describes multiple stations and should not be used.");
        }

        public static class Ril100
        {
            public static Error InvalidLength => Error.Validation(
                "Station.Ril100.InvalidLength",
                "The ril 100 must be between 2 and 5 characters long");

            public static Error InvalidFormat => Error.Validation(
                "Station.Ril100.InvalidFormat",
                "The ril 100 can only contain characters and spaces");
        }
    }
}