using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Abstractions.Authentication;

public interface ITokenGenerator
{
    string GenerateJwtToken(UserId userId, Email email);
    
    RefreshToken GenerateRefreshToken();
}