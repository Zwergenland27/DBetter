using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class TrainComposition
    {
        public static Error NotFound => Error.NotFound("TrainComposition.NotFound", "The train composition could not be found.");

        public static Error NotFindable => Error.NotFound("TrainComposition.NotFound",
            "The train composition is not findable by the given conditions");

        public static Error InPast => Error.NotFound("TrainComposition.InPast",
            "The requested train composition cannot be searched as its in the past.");
        
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("TrainComposition.Id.Invalid", $"{value} is no valid guid.");
        }
    }
}