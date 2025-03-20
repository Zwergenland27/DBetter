using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class TrainRun
    {
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("TrainRun.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class TrainRunDate
        {
            public static Error Invalid(string value) => Error.Validation("TrainRun.Date.Invalid", $"{value} is no valid date.");
        }
    }
}