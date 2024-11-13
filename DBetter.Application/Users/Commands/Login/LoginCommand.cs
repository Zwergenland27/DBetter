using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users.Commands.Login;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.Login;

public class LoginRequestBuilder : IRequestBuilder<LoginParameters, LoginCommand>
{
    public ValidatedRequiredProperty<LoginCommand> Configure(RequiredPropertyBuilder<LoginParameters, LoginCommand> builder)
    {
        var email = builder.ClassProperty(r => r.Email)
            .Required(ApplicationErrors.User.Login.Email.Missing)
            .Map(p => p.Email, Email.Create);
        
        var password = builder.ClassProperty(r => r.Password)
            .Required(ApplicationErrors.User.Login.Password.Missing)
            .Map(p => p.Password, Password.Create);

        return builder.Build(() => new LoginCommand(email, password));
    }
}

public record LoginCommand(
    Email Email,
    Password Password) : ICommand<LoginResult>;