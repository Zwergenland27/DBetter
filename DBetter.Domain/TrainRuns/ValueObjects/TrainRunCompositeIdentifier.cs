using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Domain.TrainRuns.ValueObjects;

public record TrainRunCompositeIdentifier(TrainId TrainId, TimeTablePeriod TimeTablePeriod, OperatingDay OperatingDay);