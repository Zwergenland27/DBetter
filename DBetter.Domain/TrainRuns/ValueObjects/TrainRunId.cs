using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainRuns.ValueObjects;

public record TrainRunId(Guid Value)
{
    public static TrainRunId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<TrainRunId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new TrainRunId(guid);
        }

        return DomainErrors.TrainRun.Id.Invalid(value);
    }
};