using CleanDomainValidation.Domain;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainRun.ValueObjects;

public record TrainInformation(TransportProduct Product, TrainLine? Line, TrainNumber? Number)
{
    public TrainInformation UpdateTrainNumber(TrainNumber number)
    {
        return this with { Number = number };
    }
}