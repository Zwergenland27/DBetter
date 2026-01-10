using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionRequestRoute
{
    public required Station Origin { get; init; }
    public required MeansOfTransport MeansOfTransportFirstSection { get; init; }
    public required SuggestionRequestStopover? FirstStopover { get; init; }
    public required SuggestionRequestStopover? SecondStopover { get; init; }
    public required Station Destination { get; init; }
    public required TransferAmount MaxTransfers { get; init; }
    public required TransferTime MinTransferTime { get; init; }
}