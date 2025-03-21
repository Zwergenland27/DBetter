using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.GetWithIncreasedTransferTime;

public class GetWithIncreasedTransferTimeParameters : IParameters
{
    /// <summary>
    /// Identifier of the connection
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }
    
    /// <summary>
    /// Eva number of the first station of the fixed section
    /// </summary>
    /// <example>8010085</example>
    public string? FixedStartEvaNumber { get; set; }
    
    /// <summary>
    /// Start time of the fixed section
    /// </summary>
    public DateTime? FixedStartTime { get; set; }
    
    /// <summary>
    /// Eva number of the last station of the fixed section
    /// </summary>
    /// <example>8010085</example>
    public string? FixedEndEvaNumber { get; set; }
    
    /// <summary>
    /// End time of the fixed section
    /// </summary>
    public DateTime? FixedEndTime { get; set; }
}