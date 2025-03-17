using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Connections.ValueObjects;

public record SectionId(Guid Value)
{
    public static SectionId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<SectionId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new SectionId(guid);
        }

        return DomainErrors.Connection.Section.Id.Invalid(value);
    }
};