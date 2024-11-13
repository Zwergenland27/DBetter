using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users;
using DBetter.Contracts.Users.Commands;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.Register;

public class RegisterRequestBuilder : IRequestBuilder<RegisterParameters, RegisterCommand>
{
    public ValidatedRequiredProperty<RegisterCommand> Configure(RequiredPropertyBuilder<RegisterParameters, RegisterCommand> builder)
    {
        var firstname = builder.ClassProperty(r => r.Firstname)
            .Required(ApplicationErrors.User.Register.Firstname.Missing)
            .Map(p => p.Firstname, Firstname.Create);
        
        var lastname = builder.ClassProperty(r => r.Lastname)
            .Required(ApplicationErrors.User.Register.Lastname.Missing)
            .Map(p => p.Lastname, Lastname.Create);
        
        var email = builder.ClassProperty(r => r.Email)
            .Required(ApplicationErrors.User.Register.Email.Missing)
            .Map(p => p.Email, Email.Create);
        
        var password = builder.ClassProperty(r => r.Password)
            .Required(ApplicationErrors.User.Register.Password.Missing)
            .Map(p => p.Password, Password.Create);
        
        var birthday = builder.ClassProperty(r => r.Birthday)
            .Required(ApplicationErrors.User.Register.Birthday.Missing)
            .Map(p => p.Birthday, Birthday.Create);

        return builder.Build(() => new RegisterCommand(firstname, lastname, email, password, birthday));
    }
}

public record RegisterCommand(
    Firstname Firstname,
    Lastname Lastname,
    Email Email,
    Password Password,
    Birthday Birthday) : ICommand<IUserResult>;