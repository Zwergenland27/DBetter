using DBetter.Domain.PlannedCoachLayouts.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainCompositions.TrainParts;

public class PlannedTrainPart
{ 
    public StationId FromStation { get; private set; } 
    
    public StationId ToStation { get; private set; }
    
    public PlannedCoachLayoutId PlannedLayoutId { get; private set; }
    
    internal PlannedTrainPart(StationId fromStation, StationId toStation, PlannedCoachLayoutId plannedLayoutId)
    {
        FromStation = fromStation;
        ToStation = toStation;
        PlannedLayoutId = plannedLayoutId;
    }
}