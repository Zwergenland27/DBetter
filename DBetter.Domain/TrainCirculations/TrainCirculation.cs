using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCirculations;

public class TrainCirculation : AggregateRoot<TrainCirculationId>
{
    public NormalizedBahnJourneyId NormalizedJourneyId { get; private init; }
    
    public ServiceInformation ServiceInformation { get; private set; }

    private TrainCirculation(
        TrainCirculationId id,
        NormalizedBahnJourneyId journeyId,
        ServiceInformation serviceInformation) : base(id)
    {
        NormalizedJourneyId =  journeyId;
        ServiceInformation = serviceInformation;
    }
    
    private TrainCirculation() : base(null!){}


    public static TrainCirculation Create(NormalizedBahnJourneyId journeyId, ServiceInformation serviceInformation)
    {
        return new TrainCirculation(TrainCirculationId.CreateNew(), journeyId, serviceInformation);
    }
    public void Update(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }
}