using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public static class RouteExtensions
{
    public static List<StationId> GetRequestedStationIds(this PlannedRoute plannedRoute)
    {
        var stationIds = new List<StationId>
        {
            plannedRoute.OriginStationId,
            plannedRoute.DestinationStationId
        };

        if (plannedRoute.FirstStopover is not null)
        {
            stationIds.Add(plannedRoute.FirstStopover.StationId);
        }

        if (plannedRoute.SecondStopover is not null)
        {
            stationIds.Add(plannedRoute.SecondStopover.StationId);
        }

        return stationIds.Distinct().ToList();
    }
}