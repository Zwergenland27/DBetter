namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Results;

/// <summary>
/// Contains connection suggestions for the requested route
/// </summary>
public class ConnectionSuggestionsDto
{
    /// <summary>
    /// Id of the connection request
    /// </summary>
    public required string RequestId { get; set; }
    /// <summary>
    /// Suggested connections
    /// </summary>
    public required List<ConnectionDto> Connections { get; set; }
    
    /// <summary>
    /// Contains pagination reference for earlier connection suggestions
    /// </summary>
    public required string? PageEarlier { get; set; }
    
    /// <summary>
    /// Contains pagination reference for later connection suggestions
    /// </summary>
    public required string? PageLater { get; set; }
}