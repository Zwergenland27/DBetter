namespace DBetter.Domain.TrainRun.ValueObjects;

/// <summary>
/// Journey Id of bahn.de
/// </summary>
/// <remarks>
/// DA Flag can be set to any date where the train run is valid -> skip between days is possible
/// </remarks>
public record BahnJourneyId(string Value);