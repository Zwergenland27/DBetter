using CleanDomainValidation.Domain;

namespace DBetter.Domain.Users.ValueObjects;

public class Lastname
{
    public string Value { get; private init; }
    
    private Lastname(string value)
    {
        Value = value;
    }
    
    public static CanFail<Lastname> Create(string value)
    {
        return new Lastname(value);
    }
}