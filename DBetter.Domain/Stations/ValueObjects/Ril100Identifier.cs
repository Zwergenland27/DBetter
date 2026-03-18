using CleanDomainValidation.Domain;

namespace DBetter.Domain.Stations.ValueObjects;

public record Ril100Identifier
{
    public string Value { get; private init; }

    private Ril100Identifier(string value)
    {
        Value = value;
    }
    
    public static CanFail<Ril100Identifier> Create(string value)
    {
        value = value.Trim().ToUpper();
        if (!value.All(c => char.IsLetter(c) || c is ' '))
        {
            return Errors.DomainErrors.Station.Ril100.InvalidFormat;
        }
        var length =  value.Length;
        if (length is < 2 or > 5)
        {
            return Errors.DomainErrors.Station.Ril100.InvalidLength;
        }
        return new Ril100Identifier(value);
    }
}