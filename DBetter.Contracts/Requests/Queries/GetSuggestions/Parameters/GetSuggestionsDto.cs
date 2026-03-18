using System.Text.Json.Serialization;
using CleanDomainValidation.Application;

namespace DBetter.Contracts.Requests.Queries.GetSuggestions.Parameters;

public class GetSuggestionsDto : IParameters
{
    /// <summary>
    /// Id of the request
    /// </summary>
    [JsonIgnore]
    public string? Id { get; set; }
    
    /// <summary>
    /// Identifier of the user that created the request
    /// </summary>
    [JsonIgnore]
    public string? UserId { get; set; }
    
    /// <summary>
    /// Pagination reference
    /// </summary>
    [JsonIgnore]
    public string? SuggestionMode { get; set; }
}