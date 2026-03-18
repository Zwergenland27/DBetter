using DBetter.Contracts.Shared.DTOs;

namespace DBetter.Contracts.Requests.Queries.GetSuggestions.Results;

/// <summary>
/// A connection from origin to destination station
/// </summary>
public class ConnectionResponse
{
    /// <summary>
    /// Id of the connection
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    /// Indicates that the connection starts from a different station then requested
    /// </summary>
    public required bool DifferentOrigin { get; set; }
    
    /// <summary>
    /// Indicates that the connection ends at a different station then requested
    /// </summary>
    public required bool DifferentDestination { get; set; }
    
    /// <summary>
    /// Demand information
    /// </summary>
    public required DemandResponse Demand { get; set; }
    
    /// <summary>
    /// Sections of the connection
    /// </summary>
    public required List<SegmentResponse> Segments { get; set; }
    
    /// <summary>
    /// Offer for the section, if available
    /// </summary>
    public required OfferResponse? Offer { get; set; }
    
    /// <summary>
    /// Url to bahn.de to view and buy the connection
    /// </summary>
    public required string BahnDeUrl { get; set; }
}