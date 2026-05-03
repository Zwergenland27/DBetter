using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.TrainCompositions.TrainParts;

public class PlannedTrainPartIdentifier
{
    private record Interval(RouteStopSnapshot Left, RouteStopSnapshot Right);

    private record UnambiguousCoachLayoutId(PlannedCoachLayoutId LayoutId, int Index);
    
    private readonly ImmutableList<RouteStopSnapshot> _orderedRoute;
    private readonly Dictionary<StationId, List<UnambiguousCoachLayoutId>> _observations;

    public PlannedTrainPartIdentifier(List<RouteStopSnapshot> route) 
    {
        _orderedRoute = route.OrderBy(r => r.RouteIndex).ToImmutableList();
        _observations = [];
    }

    public void AddObservation(StationId stationId, List<PlannedCoachLayoutId> observedLayouts)
    {
        var unambigousCoachLayoutIds = new List<UnambiguousCoachLayoutId>();
        foreach (var observedLayoutId in observedLayouts)
        {
            var existing = unambigousCoachLayoutIds.FirstOrDefault(unambiguousCoachLayoutId =>
                observedLayoutId == unambiguousCoachLayoutId.LayoutId);
            if (existing is null)
            {
                unambigousCoachLayoutIds.Add(new UnambiguousCoachLayoutId(observedLayoutId, 0));
            }
            else {
                unambigousCoachLayoutIds.Add(new UnambiguousCoachLayoutId(observedLayoutId, existing.Index + 1));
            }
        }
        
        _observations[stationId] = unambigousCoachLayoutIds;
    }

    /// <summary>
    /// Tries to build the train parts of the journey
    /// </summary>
    /// <param name="departureStationToScrape"></param>
    /// <param name="plannedTrainParts">All train parts, when unambiguous configuration has been found</param>
    /// <returns>True, if an unambiguous configuration has been found</returns>
    public bool Resolve(
        [MaybeNullWhen(true)] out StationId departureStationToScrape,
        [MaybeNullWhen(true)] out StationId arrivalStationToScrape,
        [MaybeNullWhen(false)] out List<PlannedTrainPart> plannedTrainParts)

    {
        var orderedRoute = _orderedRoute.OrderBy(r => r.RouteIndex).ToList();
        var firstDepartureStop = orderedRoute.First();
        var lastDepartureStop = orderedRoute[^2];

        //plannedSnapshots must contain first station
        if (!_observations.ContainsKey(firstDepartureStop.StationId))
        {
            departureStationToScrape = firstDepartureStop.StationId;
            arrivalStationToScrape = orderedRoute[1].StationId;
            plannedTrainParts = null;
            return false;
        }

        //plannedSnapshots must contain last station
        if (!_observations.ContainsKey(lastDepartureStop.StationId))
        {
            departureStationToScrape = lastDepartureStop.StationId;
            arrivalStationToScrape = orderedRoute[^1].StationId;
            plannedTrainParts = null;
            return false;
        }

        var openIntervals = BuildInitialIntervals();
        while (openIntervals.Any())
        {
            var interval = openIntervals.Dequeue();
            var leftLayout = _observations[interval.Left.StationId];
            var rightLayout = _observations[interval.Right.StationId];
            if (IsTrainPartsEqual(leftLayout, rightLayout))
            {
                continue;
            }
            
            var pivotStation = CalculatePivotStation(interval.Left, interval.Right);
            if (pivotStation is null)
            {
                continue;
            }

            if (!_observations.ContainsKey(pivotStation.StationId))
            {
                departureStationToScrape = pivotStation.StationId;
                arrivalStationToScrape = _orderedRoute[pivotStation.RouteIndex + 1].StationId;
                plannedTrainParts = null;
                return false;
            }
            
            openIntervals.Enqueue(interval with { Right = pivotStation });
            openIntervals.Enqueue(interval with { Left = pivotStation });
        }

        departureStationToScrape = null;
        arrivalStationToScrape = null;
        plannedTrainParts = ExtractTrainParts();
        return true;
    }

    private List<PlannedTrainPart> ExtractTrainParts()
    {
        var trainParts = new List<PlannedTrainPart>();

        var unambiguousLayoutIds = _observations
            .SelectMany(o => o.Value)
            .DistinctBy(s => new {s.LayoutId, s.Index});

        foreach (var layoutId in unambiguousLayoutIds)
        {
            var observedStationIds = _observations
                .Where(o => o.Value.Contains(layoutId))
                .Select(o => o.Key);
            
            var routeStops = _orderedRoute
                .Where(s => observedStationIds.Contains(s.StationId))
                .ToList();
            
            var firstStop = routeStops.MinBy(s => s.RouteIndex)!;
            var lastStopWithDeparture = routeStops.MaxBy(s => s.RouteIndex)!;
            var lastStop = lastStopWithDeparture;
            if (lastStopWithDeparture.RouteIndex != _orderedRoute.Last().RouteIndex)
            {
                lastStop = _orderedRoute[lastStopWithDeparture.RouteIndex + 1];
            }
            
            trainParts.Add(new PlannedTrainPart(
                firstStop.StationId,
                lastStop.StationId,
                layoutId.LayoutId));
        }
        
        return trainParts;
    }

    private Queue<Interval> BuildInitialIntervals()
    {
        var observedStops = _orderedRoute
            .Where(s => _observations.ContainsKey(s.StationId))
            .OrderBy(r => r.RouteIndex)
            .ToList();

        var queue = new Queue<Interval>();
        for (int i = 0; i < observedStops.Count - 1; i++)
        {
            queue.Enqueue(new Interval(observedStops[i], observedStops[i+1]));
        }

        return queue;
    }

    private RouteStopSnapshot? CalculatePivotStation(RouteStopSnapshot left, RouteStopSnapshot right)
    {
        var candidates = _orderedRoute
            .Where(s => s.RouteIndex > left.RouteIndex && s.RouteIndex < right.RouteIndex &&
                        !_observations.ContainsKey(s.StationId))
            .OrderBy(s => s.RouteIndex)
            .ToList();

        if (!candidates.Any()) return null;

        var midIndex = (left.RouteIndex + right.RouteIndex) / 2;
        return candidates.MinBy(s => Math.Abs(s.RouteIndex - midIndex));
    }

    private bool IsTrainPartsEqual(List<UnambiguousCoachLayoutId> a, List<UnambiguousCoachLayoutId> b)
    {
        var idsA = a.Select(v => $"{v.LayoutId.Value}-{v.Index}").ToHashSet();
        var idsB = b.Select(v => $"{v.LayoutId.Value}-{v.Index}").ToHashSet();
        
        return idsB.SetEquals(idsA);
    }
}