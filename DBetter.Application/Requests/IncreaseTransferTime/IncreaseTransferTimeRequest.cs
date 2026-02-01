using DBetter.Application.Requests.Dtos;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;

namespace DBetter.Application.Requests.IncreaseTransferTime;

public record IncreaseTransferTimeRequest
{
    public required IncreaseTransferTimeMode Mode { get; init; }
    
    public required ConnectionContextId OriginalConnectionContextId { get; init; }
    
    public required FixedSubConnection FixedSubConnection { get; init; }
    
    public required SuggestionRequest OriginalRequest { get; init; }
}