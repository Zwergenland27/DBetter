using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Domain.TrainCirculations;

public class TrainCirculation : AggregateRoot<TrainCirculationId>
{
    public TimeTablePeriod TimeTablePeriod { get; private set; }
    public TrainId TrainId { get; private init; }
    
    public ServiceInformation ServiceInformation { get; private set; }

    private TrainCirculation(
        TrainCirculationId id,
        TimeTablePeriod timeTablePeriod,
        TrainId trainId,
        ServiceInformation serviceInformation) : base(id)
    {
        TrainId =  trainId;
        TimeTablePeriod = timeTablePeriod;
        ServiceInformation = serviceInformation;
    }
    
    private TrainCirculation() : base(null!){}


    public static TrainCirculation Create(BahnJourneyId journeyId, ServiceInformation serviceInformation)
    {
        return new TrainCirculation(TrainCirculationId.CreateNew(), TimeTablePeriod.FromOperatingDay(journeyId.OperatingDay),  journeyId.TrainId, serviceInformation);
    }
    public void Update(ServiceNumber newServiceNumber)
    {
        ServiceInformation = ServiceInformation.UpdateServiceNumber(newServiceNumber);
    }
}