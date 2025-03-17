using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Journey.ValueObjects;

public record JourneyId(Guid Value)
{
    public static JourneyId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<JourneyId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new JourneyId(guid);
        }

        return DomainErrors.Journey.Id.Invalid(value);
    }
};