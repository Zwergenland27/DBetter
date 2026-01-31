using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record TrainId(int Value)
{
    public static CanFail<TrainId> Create(string value)
    {
        if (int.TryParse(value, out int result))
        {
            return new TrainId(result);
        }
        
        return DomainErrors.TrainCirculation.TrainId.Invalid(value);
    }
}