using CleanDomainValidation.Domain;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class User
    {
        public static Error Exists => Error.Conflict("User.Exists", "User with this email already exists");
        
        public static Error InvalidCredentials => Error.Forbidden("User.InvalidCredentials", "Invalid login credentials");
        
        public static class Id
        {
            public static Error Invalid => Error.Validation("User.Id.Invalid", "Invalid User Id");
        }

        public static class Birthday
        {
            public static Error InFuture => Error.Validation("User.Birthday.InFuture", "Birthday cannot be in the future");
        }

        public static class Discount
        {
            public static Error AlreadyExists => Error.Conflict("User.Discount.AlreadyExists", "Cannot own two discounts of the same type at the same time");
        }
    }
}