using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainCompositions.ValueObjects;

public record TrainCompositionId(Guid Value)
{
    public static TrainCompositionId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<TrainCompositionId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new TrainCompositionId(guid);
        }

        return DomainErrors.TrainComposition.Id.Invalid(value);
    }
}