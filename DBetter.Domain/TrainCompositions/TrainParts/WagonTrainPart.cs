using DBetter.Domain.Consists.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainCompositions.TrainParts;

public class WagonTrainPart: ResolvedTrainPart
{
    public ConsistId ConsistId { get; set; }
    
    public WagonTrainPart(WagonTrainPart original) : base(original)
    {
        ConsistId = original.ConsistId;
    }

    public WagonTrainPart(StationId fromStation, StationId toStation, ConsistId consistId) : base(fromStation, toStation)
    {
        ConsistId = consistId;
    }
}