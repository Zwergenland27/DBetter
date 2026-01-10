using DBetter.Application.Requests.GetSuggestions;
using DBetter.Domain.Connections.Snapshots;

namespace DBetter.Application.Requests;

public interface IExternalConnectionProvider
{
    Task<SuggestionResponse> GetSuggestions(SuggestionRequest request);
}