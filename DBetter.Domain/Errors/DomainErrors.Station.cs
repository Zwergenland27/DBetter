using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Station
    {
        public static class EvaNumber
        {
            public static Error NotNumeric(string value) => Error.Validation(
                "Station.EvaNumber.Invalid", 
                $"{value} is not a numeric value.");
        }
        public static class Name
        {
            public static Error MetaStation(string value) => Error.Conflict(
                "Station.Name.MetaStation", 
                $"{value} is a meta station that describes multiple stations and should not be used.");
        }
    }
}