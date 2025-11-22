using CleanDomainValidation.Domain;

namespace DBetter.Domain.Users.ValueObjects;

public class Email
{
    public string Value { get; private init; }
    
    private Email(string value)
    {
        Value = value;
    }
    
    public static CanFail<Email> Create(string value)
    {
        return new Email(value);
    }
}