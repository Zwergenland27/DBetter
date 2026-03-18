using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class TrainCirculation
    {
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("TrainCirculation.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class TrainId
        {
            public static Error Invalid(string value) => Error.Validation("TrainCirculation.TrainId.Invalid", $"{value} is no valid train id.");
        }
    }
}