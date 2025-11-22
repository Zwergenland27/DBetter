using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Shared;

/// <summary>
/// Demand for first and second class
/// </summary>
public record Demand(DemandStatus FirstClass, DemandStatus SecondClass);