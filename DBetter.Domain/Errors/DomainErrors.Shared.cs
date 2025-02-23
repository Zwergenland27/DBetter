using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Shared
    {
        public static class Birthday
        {
            public static Error InFuture => Error.Validation("Birthday.InFuture", "Birthday cannot be in the future");
        }

        public static class Class
        {
            public static Error Invalid => Error.Validation("Class.Invalid", "The class is invalid");
        }

        public static class DiscountType
        {
            public static Error Invalid => Error.Validation("DiscountType.Invalid", "The discount type is invalid");
        }
    }
}