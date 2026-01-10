namespace DBetter.Contracts.Shared.DTOs;

/// <summary>
/// Demand information
/// </summary>
public class DemandResponse
{
    /// <summary>
    /// Demand of first class
    /// </summary>
    public required string FirstClass { get; set; }
    
    /// <summary>
    /// Demand of second class
    /// </summary>
    public required string SecondClass { get; set; }
}