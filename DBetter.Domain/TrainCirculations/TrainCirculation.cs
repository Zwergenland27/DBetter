using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Domain.TrainCirculations;

public class TrainCirculation : AggregateRoot<TrainCirculationId>
{
    public TrainCirculationIdentifier Identifier { get; private set; }
    public ServiceInformation ServiceInformation { get; private set; }

    internal TrainCirculation(
        TrainCirculationId id,
        TrainCirculationIdentifier identifier,
        ServiceInformation serviceInformation) : base(id)
    {
        Identifier = identifier;
        ServiceInformation = serviceInformation;
    }


    public static TrainCirculation Create(BahnJourneyId journeyId, ServiceInformation serviceInformation)
    {
        return new TrainCirculation(TrainCirculationId.CreateNew(), journeyId.TrainCirculationIdentifier, serviceInformation);
    }
    public void Update(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }

    public void Update(ServiceInformation newServiceInformation)
    {
        ServiceInformation = newServiceInformation;
    }

    public void Update(LineNumber newLineNumber)
    {
        ServiceInformation = ServiceInformation.UpdateLineNumber(newLineNumber);
    }
}