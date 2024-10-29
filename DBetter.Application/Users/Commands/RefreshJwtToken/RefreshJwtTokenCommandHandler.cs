using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions;
using DBetter.Application.Abstractions.Authentication;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.RefreshJwtToken;

public class RefreshJwtTokenCommandHandler(
    IUserRepository repository,
    ITokenGenerator tokenGenerator) : ICommandHandler<RefreshJwtTokenCommand, Tuple<string, RefreshToken>>
{
    public async Task<CanFail<Tuple<string, RefreshToken>>> Handle(RefreshJwtTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(request.Id);
        if(user is null) return DomainErrors.User.InvalidCredentials;

        if (!user.IsValidRefreshToken(request.RefreshToken)) return DomainErrors.User.InvalidCredentials;
        
        var token = tokenGenerator.GenerateJwtToken(user.Id, user.Email);
        var refreshToken = tokenGenerator.GenerateRefreshToken();
        
        user.SetRefreshToken(refreshToken);
        
        return new Tuple<string, RefreshToken>(token, refreshToken);
    }
}