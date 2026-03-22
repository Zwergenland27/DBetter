using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Tests;

public record JourneyIdValues(
    string JourneyId,
    EvaNumber Origin,
    EvaNumber Destination,
    OperatingDay OperatingDay,
    TrainCirculationIdentifier TrainCirculationIdentifier);