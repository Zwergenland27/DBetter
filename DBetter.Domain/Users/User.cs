using System.Security.Cryptography;
using System.Text;
using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.Users;

public class User : AggregateRoot<UserId>
{
    private string _passwordHash;

    private string _passwordSalt;
    
    private RefreshToken? _refreshToken;
    public Firstname Firstname { get; private set; }
    
    public Lastname Lastname { get; private set; }
    
    public Email Email { get; private set; }
    
    private User(
        UserId id,
        Firstname firstname,
        Lastname lastname,
        Email email,
        string passwordHash,
        string passwordSalt) : base(id)
    {
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        _passwordHash = passwordHash;
        _passwordSalt = passwordSalt;
    }
    
    public static CanFail<User> Register(Firstname firstname, Lastname lastname, Email email, Password password)
    {
        string passwordHash;
        string passwordSalt;
        
        using (var hmac = new HMACSHA256())
        {
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password.Value)));
        }
        
        var user = new User(UserId.CreateNew(), firstname, lastname, email, passwordHash, passwordSalt);

        return user;
    }
    
    public void SetRefreshToken(RefreshToken refreshToken)
    {
        _refreshToken = refreshToken;
    }

    public bool IsValidRefreshToken(string token)
    {
        if (_refreshToken is null) return false;
        return _refreshToken.IsValid(token);
    }

    public bool IsValidPassword(Password password)
    {
        using var hmac = new HMACSHA256(Convert.FromBase64String(_passwordSalt));
        var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password.Value)));
        return computedHash == _passwordHash;
    }
    
    public CanFail EditPersonalData(
        Firstname? firstname,
        Lastname? lastname,
        Email? email)
    {
        if (firstname is not null) Firstname = firstname;
        if (lastname is not null) Lastname = lastname;
        if (email is not null) Email = email;
        
        return CanFail.Success;
    }
}