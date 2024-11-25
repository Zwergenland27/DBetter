namespace DBetter.Contracts.Users;

public class AuthenticationDto
{
    public required String Token { get; set; }
    
    public required DateTime RefreshTokenExpiration { get; set; }
}