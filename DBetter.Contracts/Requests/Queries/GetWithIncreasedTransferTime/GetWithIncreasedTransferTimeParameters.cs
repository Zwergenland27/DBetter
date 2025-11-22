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
    
    [JsonIgnore]
    public string? RouteId { get; set; }
    
    [JsonIgnore]
    public int? TransferId { get; set; }
    
    [JsonIgnore]
    public string? Mode { get; set; }
}