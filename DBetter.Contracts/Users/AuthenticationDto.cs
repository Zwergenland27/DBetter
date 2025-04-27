namespace DBetter.Contracts.Users;

public class AuthenticationDto
{
    public required string Token { get; set; }
    
    public required string RefreshTokenExpiration { get; set; }
}