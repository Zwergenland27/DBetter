using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanMediator.Commands;
using DBetter.Contracts.Users.Commands.Login;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.Login;

public class LoginRequestBuilder : IRequestBuilder<LoginParameters, LoginCommand>
{
    public ValidatedRequiredProperty<LoginCommand> Configure(RequiredPropertyBuilder<LoginParameters, LoginCommand> builder)
    {
        var email = builder.ClassProperty(r => r.Email)
            .Required()
            .Map(p => p.Email, Email.Create);
        
        var password = builder.ClassProperty(r => r.Password)
            .Required()
            .Map(p => p.Password, Password.Create);

        return builder.Build(() => new LoginCommand(email, password));
    }
}

public record LoginCommand(
    Email Email,
    Password Password) : ICommand<Tuple<String, RefreshToken>>;