using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.Stations.ValueObjects;

public record StationId(Guid Value)
{
    public static StationId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<StationId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new StationId(guid);
        }

        return DomainErrors.Station.Id.Invalid(value);
    }
}