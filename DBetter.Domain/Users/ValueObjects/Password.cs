using CleanDomainValidation.Domain;

namespace DBetter.Domain.Users.ValueObjects;

public class Password
{
    public string Value { get; private init; }
    
    private Password(string value)
    {
        Value = value;
    }
    
    public static CanFail<Password> Create(string value)
    {
        return new Password(value);
    }
}