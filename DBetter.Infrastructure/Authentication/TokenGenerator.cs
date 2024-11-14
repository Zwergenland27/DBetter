using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DBetter.Application.Abstractions.Authentication;
using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DBetter.Infrastructure.Authentication;

public class TokenGenerator(IOptions<JwtSettings> options) : ITokenGenerator
{
    private readonly JwtSettings _jwtSettings = options.Value;
    public string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        List<Claim> claims =
        [
            new (ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new (ClaimTypes.Email, user.Email.Value),
            new (ClaimTypes.GivenName, user.Firstname.Value),
            new (ClaimTypes.Surname, user.Lastname.Value),
        ];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = credentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(4)),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays)
        };
    }
}