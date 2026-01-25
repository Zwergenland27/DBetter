using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record TrainCirculationId(Guid Value)
{
    public static TrainCirculationId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<TrainCirculationId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new TrainCirculationId(guid);
        }

        return DomainErrors.TrainCirculation.Id.Invalid(value);
    }
};