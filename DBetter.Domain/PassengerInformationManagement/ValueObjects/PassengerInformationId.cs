using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.PassengerInformationManagement.ValueObjects;

public record PassengerInformationId(Guid Value)
{
    public static PassengerInformationId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<PassengerInformationId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new PassengerInformationId(guid);
        }

        return DomainErrors.PassengerInformation.Id.Invalid(value);
    }
}