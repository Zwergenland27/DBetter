using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions;
using DBetter.Application.Abstractions.Authentication;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.Login;

public class LoginCommandHandler(
    IUserRepository repository,
    ITokenGenerator tokenGenerator) : ICommandHandler<LoginCommand, Tuple<string, RefreshToken>>
{
    public async Task<CanFail<Tuple<string, RefreshToken>>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email);
        
        if (user is null) return DomainErrors.User.InvalidCredentials;
        
        if(!user.IsValidPassword(request.Password)) return DomainErrors.User.InvalidCredentials;
        
        var token = tokenGenerator.GenerateJwtToken(user.Id, user.Email);
        var refreshToken = tokenGenerator.GenerateRefreshToken();
        
        user.SetRefreshToken(refreshToken);
        
        return new Tuple<string, RefreshToken>(token, refreshToken);
    }
}