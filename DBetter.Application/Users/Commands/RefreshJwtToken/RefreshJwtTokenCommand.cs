using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.RefreshJwtToken;

public class RefreshJwtTokenRequestBuilder : IRequestBuilder<RefreshJwtTokenParameters, RefreshJwtTokenCommand>
{
    public ValidatedRequiredProperty<RefreshJwtTokenCommand> Configure(RequiredPropertyBuilder<RefreshJwtTokenParameters, RefreshJwtTokenCommand> builder)
    {
        var email = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, UserId.Create);
        
        var refreshToken = builder.ClassProperty(r => r.RefreshToken)
            .Required()
            .Map(p => p.RefreshToken);

        return builder.Build(() => new RefreshJwtTokenCommand(email, refreshToken));
    }
}

public record RefreshJwtTokenCommand(UserId Id, string RefreshToken) : ICommand<Tuple<String, RefreshToken>>;