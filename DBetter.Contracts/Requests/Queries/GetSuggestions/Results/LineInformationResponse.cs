namespace DBetter.Contracts.Requests.Queries.GetSuggestions.Results;

public class LineInformationResponse
{
    public required string? ProductClass { get; set; }
    
    public required string Number { get; set; }
    
    public required int? ServiceNumber { get; set; }
}