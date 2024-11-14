using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Abstractions.Authentication;

public interface ITokenGenerator
{
    string GenerateJwtToken(User user);
    
    RefreshToken GenerateRefreshToken();
}