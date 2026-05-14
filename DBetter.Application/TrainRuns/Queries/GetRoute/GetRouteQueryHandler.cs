using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Contracts.TrainRuns.Queries.GetRoute;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;
using DBetter.Domain.Stations;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainRuns;

namespace DBetter.Application.TrainRuns.Queries.GetRoute;

public class GetRouteQueryHandler(
    ITrainRunRepository trainRunRepository,
    ITrainCirculationRepository trainCirculationRepository,
    IStationRepository stationRepository,
    IRouteRepository routeRepository): QueryHandlerBase<GetRouteQuery, RouteResponse>
{
    public override async Task<CanFail<RouteResponse>> Handle(GetRouteQuery query, CancellationToken cancellationToken)
    {
        var trainRun = await trainRunRepository.GetAsync(query.TrainRunId);
        if (trainRun is null) return DomainErrors.TrainRun.NotFound(query.TrainRunId);
        var route = await routeRepository.GetAsync(query.TrainRunId);
        if (route is null) return DomainErrors.Route.NotFound;

        var trainCirculation = await trainCirculationRepository.GetAsync(trainRun.CirculationId);
        if (trainCirculation is null) throw new InvalidOperationException("Train Circulation does not exist");
        
        var stations = await stationRepository.GetManyAsync(route.Stops.Select(s => s.StationId));

        return new RouteResponse
        {
            ServiceNumber = trainCirculation.ServiceInformation.ServiceNumber?.Value,
            Stops = BuildRoute(route, stations)
        };
    }

    private List<RouteStopResponse> BuildRoute(Route route, List<Station> stations)
    {
        var stops = new List<RouteStopResponse>();
        var stopIndex = 0;
        foreach (var stop in route.Stops)
        {
            var evaNumber = stations.First(s => s.Id == stop.StationId).EvaNumber;
            stops.Add(new RouteStopResponse
            {
                Id = stop.StationId.Value.ToString(),
                EvaNumber = evaNumber.Value,
                DepartureTime = stop.DepartureTime?.ToResponse(),
                ArrivalTime = stop.ArrivalTime?.ToResponse(),
                Platform = stop.Platform?.ToResponse(),
                RouteIndex = stopIndex
            });
            stopIndex++;
        }
        return stops;
    }
}