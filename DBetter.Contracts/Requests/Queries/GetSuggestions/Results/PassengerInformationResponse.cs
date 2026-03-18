namespace DBetter.Contracts.Requests.Queries.GetSuggestions.Results;

public class PassengerInformationResponse
{
    public required string Type { get; init; }
    
    public required string Message { get; init; }
    
    public required string Priority { get; init; }
}