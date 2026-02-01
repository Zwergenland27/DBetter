namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record TimeTableCompositeIdentifier(TrainId TrainId, TimeTablePeriod TimeTablePeriod);