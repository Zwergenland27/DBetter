using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainRuns.ValueObjects;

public record TrainRunPassengerInformationId(Guid Value)
{
    public static TrainRunPassengerInformationId CreateNew()
    {
        return new(Guid.NewGuid());
    }

    public static CanFail<TrainRunPassengerInformationId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new TrainRunPassengerInformationId(guid);
        }

        return DomainErrors.TrainRun.PassengerInformation.Id.Invalid(value);
    }
}