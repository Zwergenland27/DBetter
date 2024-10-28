using CleanDomainValidation.Domain;

namespace DBetter.Application.Errors;

public static partial class ApplicationErrors
{
    public static class User
    {
        public static class Create
        {
            public static class Firstname
            {
                public static Error Missing => Error.Validation("User.Create.Firstname.Missing", "Firstname is required");
            }
            
            public static class Lastname
            {
                public static Error Missing => Error.Validation("User.Create.Lastname.Missing", "Lastname is required");
            }
            
            public static class Email
            {
                public static Error Missing => Error.Validation("User.Create.Email.Missing", "Email is required");
            }
            
            public static class Password
            {
                public static Error Missing => Error.Validation("User.Create.Password.Missing", "Password is required");
            }
        }
    }
}