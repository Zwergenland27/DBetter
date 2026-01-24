using DBetter.Application.Requests.GetSuggestions;

namespace DBetter.Application.Requests;

public interface IExternalConnectionProvider
{
    Task<SuggestionResponse> GetSuggestions(SuggestionRequest request);
}