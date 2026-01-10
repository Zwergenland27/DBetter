using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Errors;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Stations;

namespace DBetter.Application.Routes.Queries.Get;

public class GetRouteQueryHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    IRouteRepository routeRepository,
    IExternalRouteProvider routeProvider) : CommandHandlerBase<GetRouteQuery, RouteResponse>
{
    private List<Station> _existingStations = [];
    private List<Station> _stationsToCreate = [];
    
    public override async Task<CanFail<RouteResponse>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var route = await routeRepository.GetAsync(request.Id);

        if (route is null) return DomainErrors.Route.NotFound(request.Id);
        
        var routeSnapshot = await routeProvider.GetRouteAsync(route.JourneyId);
        
        await ExtractStations(routeSnapshot);
        ExtractMissingStations(routeSnapshot);
        stationRepository.AddRange(_stationsToCreate);
        
        route.Update(routeSnapshot);
        if (routeSnapshot.ServiceNumbers.Any())
        {
            route.UpdateServiceNumber(routeSnapshot.ServiceNumbers.First());   
        }
        
        await unitOfWork.CommitAsync(cancellationToken);

        var responseFactory = new RouteResponseFactory(route, _existingStations);
        return responseFactory.MapToResponse(routeSnapshot);
    }

    private async Task ExtractStations(RouteSnapshot routeSnapshot)
    {
        var evaNumbers = routeSnapshot.Stops.Select(stop => stop.EvaNumber);
        _existingStations = await stationRepository.GetManyAsync(evaNumbers);
    }
    
    private void ExtractMissingStations(RouteSnapshot routeSnapshot)
    {
        var unknownStations = routeSnapshot.GetUnknownStations(_existingStations);
        foreach (var station in unknownStations)
        {
            var newStation = Station.CreateFromSnapshot(station);
            _stationsToCreate.Add(newStation);
            _existingStations.Add(newStation);
        }
    }
}