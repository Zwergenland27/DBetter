using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class User
    {
        public static Error Exists => Error.Conflict("User.Exists", "User with this email already exists");
        public static class Id
        {
            public static Error Invalid => Error.Validation("User.Id.Invalid", "Invalid User Id");
        }
    }
}