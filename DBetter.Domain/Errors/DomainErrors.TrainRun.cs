using CleanDomainValidation.Domain;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class TrainRun
    {
        public static Error NotFound(TrainRunId id) => Error.NotFound("TrainRun.NotFound",
            $"A train run with the id {id.Value} was not found.");
        
        public static Error NotSupported => Error.NotFound("TrainRun.NotSupported",
            "The train run could not be found, possibly because it is a replacement train, which is currently not supported.");
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("TrainRun.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class ServiceNumber
        {
            public static Error Invalid(string value) => Error.Validation("TrainRun.ServiceNumber.Invalid", 
                $"{value} is not a valid service number.");
        }

        public static class PassengerInformation
        {
            public static class Id
            {
                public static Error Invalid(string value) => Error.Validation("TrainRun.PassengerInformation.Id.Invalid", $"{value} is no valid guid.");
            }
        }
    }
}