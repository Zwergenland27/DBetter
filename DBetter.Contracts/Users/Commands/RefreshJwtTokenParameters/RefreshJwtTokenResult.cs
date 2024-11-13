namespace DBetter.Contracts.Users.Commands.RefreshJwtTokenParameters;

public class RefreshJwtTokenResult
{
    public required string TokenType { get; set; }
    
    public required string AccessToken { get; set; }
}