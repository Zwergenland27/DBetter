using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Requests.Queries.GetWithIncreasedTransferTime;

public class GetWithIncreasedTransferTimeDto : IParameters
{
    [JsonIgnore]
    public string? UserId { get; set; }
    
    [JsonIgnore]
    public string? ConnectionRequestId { get; set; }
    
    [JsonIgnore]
    public string? ConnectionId { get; set; }
    
    [JsonIgnore]
    public byte? TransferId { get; set; }
    
    [JsonIgnore]
    public string? Mode { get; set; }
}