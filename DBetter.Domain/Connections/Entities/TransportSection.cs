using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.TrainRun.ValueObjects;

namespace DBetter.Domain.Connections.Entities;

public class TransportSection : Section
{
    public Demand Demand { get; private set; }
    public TrainRunId TrainRunId { get; private init; }
    
    public StopIndex FromStop { get; private init; }
    
    public StopIndex ToStop { get; private init; }
    
    private TransportSection(){}

    private TransportSection(
        SectionId id,
        Demand demand,
        TrainRunId trainRunId,
        StopIndex fromStop,
        StopIndex toStop) : base(id)
    {
        Demand = demand;
        TrainRunId = trainRunId;
        FromStop = fromStop;
        ToStop = toStop;
    }

    public static TransportSection Create(
        Demand demand,
        TrainRunId trainRunId,
        StopIndex fromStop,
        StopIndex toStop)
    {
        return new TransportSection(
            SectionId.CreateNew(),
            demand,
            trainRunId,
            fromStop,
            toStop);
    }
}