using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record ServiceInformation(TransportCategory TransportCategory, LineNumber? LineNumber, ServiceNumber? ServiceNumber)
{
    public ServiceInformation UpdateServiceNumber(ServiceNumber number)
    {
        return this with { ServiceNumber = number };
    }
}