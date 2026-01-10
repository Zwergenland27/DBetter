using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations;

namespace DBetter.Application.Requests.GetSuggestions;

public record SuggestionRequestStopover
{
    public required Station Station { get; init; }
    public required int StayDuration { get; init; }
    public required MeansOfTransport MeansOfTransportNextSection { get; init; }
}