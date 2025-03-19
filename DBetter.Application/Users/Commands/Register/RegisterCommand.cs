using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users;
using DBetter.Contracts.Users.Commands;
using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.Register;

public class RegisterRequestBuilder : IRequestBuilder<RegisterParameters, RegisterCommand>
{
    public ValidatedRequiredProperty<RegisterCommand> Configure(RequiredPropertyBuilder<RegisterParameters, RegisterCommand> builder)
    {
        var firstname = builder.ClassProperty(r => r.Firstname)
            .Required()
            .Map(p => p.Firstname, Firstname.Create);
        
        var lastname = builder.ClassProperty(r => r.Lastname)
            .Required()
            .Map(p => p.Lastname, Lastname.Create);
        
        var email = builder.ClassProperty(r => r.Email)
            .Required()
            .Map(p => p.Email, Email.Create);
        
        var password = builder.ClassProperty(r => r.Password)
            .Required()
            .Map(p => p.Password, Password.Create);
        
        var birthday = builder.ClassProperty(r => r.Birthday)
            .Required()
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