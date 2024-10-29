namespace DBetter.Domain.Users.ValueObjects;

public class RefreshToken
{
    public string Token { get; set; }
    
    public DateTime Created { get; private init; } = DateTime.UtcNow;
    
    public DateTime Expires { get; set; }
    
    public bool IsValid(string token) => Token == token && DateTime.UtcNow <= Expires;
}