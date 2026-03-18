using CleanDomainValidation.Domain;

namespace DBetter.Domain.PassengerInformationManagement.ValueObjects;

public record PassengerInformationCode
{
    public static readonly PassengerInformationCode Unmapped = new ("UNMAPPED");
    public string Value { get; private init; }

    private PassengerInformationCode(string value)
    {
        Value = value;
    }
    
    public static CanFail<PassengerInformationCode> Create(string value)
    {
        return new PassengerInformationCode(value);
    }
}