using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record ServiceNumber(int Value)
{
    public static CanFail<ServiceNumber> Create(string value)
    {
        if (int.TryParse(value, out var intValue) && intValue > 0)
        {
            return new ServiceNumber(intValue);
        }

        return DomainErrors.TrainRun.ServiceNumber.Invalid(value);
    }
}