using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public static class RouteExtensions
{
    public static List<StationId> GetRequestedStationIds(this Route route)
    {
        var stationIds = new List<StationId>
        {
            route.OriginStationId,
            route.DestinationStationId
        };

        if (route.FirstStopover is not null)
        {
            stationIds.Add(route.FirstStopover.StationId);
        }

        if (route.SecondStopover is not null)
        {
            stationIds.Add(route.SecondStopover.StationId);
        }

        return stationIds.Distinct().ToList();
    }
}