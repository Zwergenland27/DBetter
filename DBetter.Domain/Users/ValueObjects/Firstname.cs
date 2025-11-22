using CleanDomainValidation.Domain;

namespace DBetter.Domain.Users.ValueObjects;

public record Firstname
{
    public string Value { get; private init; }
    
    private Firstname(string value)
    {
        Value = value;
    }
    
    public static CanFail<Firstname> Create(string value)
    {
        return new Firstname(value);
    }
}