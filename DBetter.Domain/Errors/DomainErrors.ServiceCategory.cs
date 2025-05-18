using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class ServiceCategory
    {
        public static class Id
        {
            public static Error Invalid => Error.Validation("ServiceCategory.Id.Invalid", "Invalid User Id");
        }
    }
}