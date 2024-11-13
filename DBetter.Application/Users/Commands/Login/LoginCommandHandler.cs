using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Authentication;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users.Commands.Login;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;

namespace DBetter.Application.Users.Commands.Login;

public class LoginCommandHandler(
    IUserRepository repository,
    ITokenGenerator tokenGenerator) : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<CanFail<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email);
        
        if (user is null) return DomainErrors.User.InvalidCredentials;
        
        if(!user.IsValidPassword(request.Password)) return DomainErrors.User.InvalidCredentials;
        
        var token = tokenGenerator.GenerateJwtToken(user.Id, user.Email);
        var refreshToken = tokenGenerator.GenerateRefreshToken();
        
        user.SetRefreshToken(refreshToken);

        return new LoginResult
        {
            TokenType = "Bearer",
            AccessToken = token,
            RefreshToken = refreshToken.Token,
            Expires = refreshToken.Expires
        };
    }
}