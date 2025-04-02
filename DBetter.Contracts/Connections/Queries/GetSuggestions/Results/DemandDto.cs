namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Demand information
/// </summary>
public class DemandDto
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