using DBetter.Domain.Abstractions;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;

namespace DBetter.Domain.TrainCirculations;

public class TrainCirculation : AggregateRoot<TrainCirculationId>
{
    public TimeTablePeriod TimeTablePeriod { get; private set; }
    public TrainId TrainId { get; private init; }
    
    public ServiceInformation ServiceInformation { get; private set; }

    internal TrainCirculation(
        TrainCirculationId id,
        TimeTablePeriod timeTablePeriod,
        TrainId trainId,
        ServiceInformation serviceInformation) : base(id)
    {
        TrainId =  trainId;
        TimeTablePeriod = timeTablePeriod;
        ServiceInformation = serviceInformation;
    }


    public static TrainCirculation Create(BahnJourneyId journeyId, ServiceInformation serviceInformation)
    {
        return new TrainCirculation(TrainCirculationId.CreateNew(), TimeTablePeriod.FromOperatingDay(journeyId.OperatingDay),  journeyId.TrainId, serviceInformation);
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