using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;

public class GetSuggestionsParameters : IParameters
{
    /// <summary>
    /// Id of the request
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }
    
    /// <summary>
    /// Pagination reference
    /// </summary>
    [JsonIgnore]
    public string? Page { get; set; }
}