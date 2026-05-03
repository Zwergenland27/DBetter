using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainCompositions.TrainParts;

public abstract class ResolvedTrainPart
{
    public StationId FromStation { get; private set; } 
    
    public StationId ToStation { get; private set; }
    
    protected ResolvedTrainPart(ResolvedTrainPart original)
    {
        FromStation = original.FromStation;
        ToStation = original.ToStation;
    }

    protected ResolvedTrainPart(StationId fromStation, StationId toStation)
    {
        FromStation = fromStation;
        ToStation = toStation;
    }
}