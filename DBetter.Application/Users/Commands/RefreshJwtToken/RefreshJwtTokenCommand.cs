using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.RefreshJwtToken;

public class RefreshJwtTokenRequestBuilder : IRequestBuilder<RefreshJwtTokenParameters, RefreshJwtTokenCommand>
{
    public ValidatedRequiredProperty<RefreshJwtTokenCommand> Configure(RequiredPropertyBuilder<RefreshJwtTokenParameters, RefreshJwtTokenCommand> builder)
    {
        var email = builder.ClassProperty(r => r.Id)
            .Required(ApplicationErrors.User.RefreshJwtToken.Id.Missing)
            .Map(p => p.Id, UserId.Create);
        
        var refreshToken = builder.ClassProperty(r => r.RefreshToken)
            .Required(ApplicationErrors.User.RefreshJwtToken.RefreshToken.Missing)
            .Map(p => p.RefreshToken);

        return builder.Build(() => new RefreshJwtTokenCommand(email, refreshToken));
    }
}

public record RefreshJwtTokenCommand(UserId Id, string RefreshToken) : ICommand<Tuple<string, RefreshToken>>;