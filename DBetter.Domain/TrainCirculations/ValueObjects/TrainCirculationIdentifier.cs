using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record TrainCirculationIdentifier(EvaNumber Origin, TimeOnly DepartureTime, EvaNumber Destination, TravelDuration Duration);