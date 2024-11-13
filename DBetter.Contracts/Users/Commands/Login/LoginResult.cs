namespace DBetter.Contracts.Users.Commands.Login;

public class LoginResult
{
    public required string TokenType { get; set; }
    
    public required string AccessToken { get; set; }
    
    public required DateTime Expires { get; set; }
    
    public required string RefreshToken { get; set; }
}