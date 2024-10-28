using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users;
using DBetter.Contracts.Users.Commands;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.RegisterCommand;

public class RegisterUserRequestBuilder : IRequestBuilder<RegisterUserParameters, RegisterUserCommand>
{
    public ValidatedRequiredProperty<RegisterUserCommand> Configure(RequiredPropertyBuilder<RegisterUserParameters, RegisterUserCommand> builder)
    {
        var firstname = builder.ClassProperty(r => r.Firstname)
            .Required(ApplicationErrors.User.Create.Firstname.Missing)
            .Map(p => p.Firstname, Firstname.Create);
        
        var lastname = builder.ClassProperty(r => r.Lastname)
            .Required(ApplicationErrors.User.Create.Lastname.Missing)
            .Map(p => p.Lastname, Lastname.Create);
        
        var email = builder.ClassProperty(r => r.Email)
            .Required(ApplicationErrors.User.Create.Email.Missing)
            .Map(p => p.Email, Email.Create);
        
        var password = builder.ClassProperty(r => r.Password)
            .Required(ApplicationErrors.User.Create.Password.Missing)
            .Map(p => p.Password, Password.Create);

        return builder.Build(() => new RegisterUserCommand(firstname, lastname, email, password));
    }
}

public record RegisterUserCommand(
    Firstname Firstname,
    Lastname Lastname,
    Email Email,
    Password Password) : ICommand<UserResult>;