namespace DBetter.Domain.Connections.ValueObjects;

/// <summary>
/// Demand for first and second class
/// </summary>
public record Demand(DemandStatus FirstClass, DemandStatus SecondClass);