using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Domain.TrainRuns.ValueObjects;

public record TrainRunIdentifier(TrainCirculationIdentifier TrainCirculation, OperatingDay OperatingDay);