namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record TimeTableCompositeIdentifier(TrainId TrainId, TimeTablePeriod TimeTablePeriod)
{
    public static TimeTableCompositeIdentifier Create(TrainId trainId, TimeTablePeriod timeTablePeriod)
    {
        return new TimeTableCompositeIdentifier(trainId, timeTablePeriod);
    }
}