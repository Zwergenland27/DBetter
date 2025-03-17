using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Connection
    {
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Connection.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class Section
        {
            public static class Id
            {
                public static Error Invalid(string value) => Error.Validation("Connection.Section.Id.Invalid", $"{value} is no valid guid.");
            }
        }
    }
}