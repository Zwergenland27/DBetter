using CleanDomainValidation.Domain;

namespace DBetter.Application.Errors;

public static partial class ApplicationErrors
{
    public static class User
    {
        public static class Register
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
            
            public static class Birthday
            {
                public static Error Missing => Error.Validation("User.Create.Birthday.Missing", "Birthday is required");
            }
            
            public static class Password
            {
                public static Error Missing => Error.Validation("User.Create.Password.Missing", "Password is required");
            }
        }

        public static class Login
        {
            public static class Email
            {
                public static Error Missing => Error.Validation("User.Login.Email.Missing", "Email is required");
            }
            
            public static class Password
            {
                public static Error Missing => Error.Validation("User.Login.Password.Missing", "Password is required");
            }
        }
        
        public static class RefreshJwtToken
        {
            public static class Id
            {
                public static Error Missing => Error.Validation("User.RefreshJwtToken.Id.Missing", "Id is required");
            }
            
            public static class RefreshToken
            {
                public static Error Missing => Error.Validation("User.RefreshJwtToken.RefreshToken.Missing", "Refresh token is required");
            }
        }
        
        public static class EditPersonalData
        {
            public static class Id
            {
                public static Error Missing => Error.Validation("User.EditPersonalData.Id.Missing", "Id is required");
            }
        }

        public class AddDiscount
        {
            public static class UserId
            {
                public static Error Missing => Error.Validation("User.AddDiscount.UserId.Missing", "User id is required");
            }
            
            public static class Type
            {
                public static Error Missing => Error.Validation("User.AddDiscount.Type.Missing", "Discount type is required");
                
                public static Error Invalid => Error.Validation("User.AddDiscount.Type.Invalid", "Discount type is invalid");
            }
            
            public static class Class
            {
                public static Error Missing => Error.Validation("User.AddDiscount.Class.Missing", "Class is required");
                
                public static Error Invalid => Error.Validation("User.AddDiscount.Class.Invalid", "Class is invalid");
            }

            public static class BoughtAtUtc
            {
                public static Error Missing =>
                    Error.Validation("User.AddDiscount.BoughtAtUtc.Missing", "Bought at is required");
            }
            
            public static class ValidUntilUtc
            {
                public static Error Missing =>
                    Error.Validation("User.AddDiscount.ValidUntilUtc.Missing", "Valid until is required");
            }
        }

        public class GetMyPassengers
        {
            public static class UserId
            {
                public static Error Missing => Error.Validation("User.GetMyPassengers.UserId.Missing", "User id is required");
            }
        }
    }
}