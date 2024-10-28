using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.Users;

public class User : AggregateRoot<UserId>
{
    public Firstname Firstname { get; private set; }
    
    public Lastname Lastname { get; private set; }
    
    public Email Email { get; private set; }
    
    public Password Password { get; private set; }
    
    private User(
        UserId id,
        Firstname firstname,
        Lastname lastname,
        Email email,
        Password password) : base(id)
    {
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        Password = password;
    }
    
    public static CanFail<User> Register(Firstname firstname, Lastname lastname, Email email, Password password)
    {
        return new User(UserId.CreateNew(), firstname, lastname, email, password);
    }
}