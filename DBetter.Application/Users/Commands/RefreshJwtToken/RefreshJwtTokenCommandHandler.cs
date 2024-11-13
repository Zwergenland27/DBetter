using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Authentication;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;

namespace DBetter.Application.Users.Commands.RefreshJwtToken;

public class RefreshJwtTokenCommandHandler(
    IUserRepository repository,
    ITokenGenerator tokenGenerator) : ICommandHandler<RefreshJwtTokenCommand, RefreshJwtTokenResult>
{
    public async Task<CanFail<RefreshJwtTokenResult>> Handle(RefreshJwtTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(request.Id);
        if(user is null) return DomainErrors.User.InvalidCredentials;

        if (!user.IsValidRefreshToken(request.RefreshToken)) return DomainErrors.User.InvalidCredentials;
        
        var token = tokenGenerator.GenerateJwtToken(user.Id, user.Email);

        return new RefreshJwtTokenResult
        {
            TokenType = "Bearer",
            AccessToken = token,
        };
    }
}