using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Authentication;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.Login;

public class LoginCommandHandler(
    IUserRepository repository,
    ITokenGenerator tokenGenerator) : CommandHandlerBase<LoginCommand, Tuple<String, RefreshToken>>
{
    public override async Task<CanFail<Tuple<String, RefreshToken>>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByEmailAsync(request.Email);
        
        if (user is null) return DomainErrors.User.InvalidCredentials;
        
        if(!user.IsValidPassword(request.Password)) return DomainErrors.User.InvalidCredentials;
        
        var token = tokenGenerator.GenerateJwtToken(user);
        var refreshToken = tokenGenerator.GenerateRefreshToken();
        
        user.SetRefreshToken(refreshToken);

        return new Tuple<String, RefreshToken>(token, refreshToken);
    }
}