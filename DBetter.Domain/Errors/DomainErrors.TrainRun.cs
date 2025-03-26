using CleanDomainValidation.Domain;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class TrainRun
    {
        public static Error NotFound(TrainRunId id) => Error.NotFound("TrainRun.NotFound",
            $"A train run with the id {id.Value} was not found.");
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("TrainRun.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class TrainRunDate
        {
            public static Error BahnInvalid(string value) => Error.Validation("TrainRun.Date.BahnInvalid", $"{value} is no valid bahn date.");
            
            public static Error Invalid(string value) => Error.Validation("TrainRun.Date.Invalid", $"{value} is no valid date.");
        }
    }
}