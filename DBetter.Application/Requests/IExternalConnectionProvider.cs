using DBetter.Application.Connections.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.Requests.IncreaseTransferTime;

namespace DBetter.Application.Requests;

public interface IExternalConnectionProvider
{
    Task<SuggestionResponse> GetSuggestions(SuggestionRequest request);
    
    Task<ConnectionDto?> GetWithIncreasedTransferTime(IncreaseTransferTimeRequest request);
}