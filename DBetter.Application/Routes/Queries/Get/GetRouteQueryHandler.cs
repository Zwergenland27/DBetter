using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Application.Abstractions.Persistence;
using DBetter.Application.Routes.Dtos;
using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Errors;
using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.Snapshots;
using DBetter.Domain.Stations;

namespace DBetter.Application.Routes.Queries.Get;

public class GetRouteQueryHandler(
    IUnitOfWork unitOfWork,
    IStationRepository stationRepository,
    IRouteRepository routeRepository,
    IExternalRouteProvider routeProvider,
    IPassengerInformationRepository passengerInformationRepository) : CommandHandlerBase<GetRouteQuery, RouteResponse>
{
    private List<Station> _existingStations = [];
    private List<Station> _stationsToCreate = [];
    private List<PassengerInformation> _existingPassengerInformation = [];
    private List<PassengerInformation> _passengerInformationToCreate = [];
    public override async Task<CanFail<RouteResponse>> Handle(GetRouteQuery request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        var route = await routeRepository.GetAsync(request.Id);

        if (route is null) return DomainErrors.Route.NotFound(request.Id);
        
        var routeSnapshot = await routeProvider.GetRouteAsync(route.JourneyId);
        _existingPassengerInformation = await passengerInformationRepository.GetManyAsync(route.PassengerInformation.Select(im => im.InformationId));
        _existingPassengerInformation.AddRange(await passengerInformationRepository.FindManyAsync(routeSnapshot.PassengerInformation.Select(pim => pim.OriginalText)));
        _existingPassengerInformation = _existingPassengerInformation.Distinct().ToList();
        
        await ExtractStations(routeSnapshot);
        ExtractMissingStations(routeSnapshot);
        ExtractMissingPassengerInformation(routeSnapshot);
        
        stationRepository.AddRange(_stationsToCreate);
        passengerInformationRepository.AddRange(_passengerInformationToCreate);
        
        route.Update(routeSnapshot.BikeCarriage);
        route.Update(routeSnapshot.Catering);
        
        var passengerInformationSnapshots = routeSnapshot.PassengerInformation
            .Select(pimDto =>
            {
                var pim = _existingPassengerInformation.First(im => im.Text == pimDto.OriginalText);
                return new RoutePassengerInformationSnapshot(pim.Id, pimDto.FromStopIndex, pimDto.ToStopIndex);
            }).ToList();
        route.ReconcilePassengerInformation(passengerInformationSnapshots);
        
        if (routeSnapshot.ServiceNumbers.Any())
        {
            route.Update(routeSnapshot.ServiceNumbers.First());   
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
            var newStation = Station.CreateFromSnapshot(station.EvaNumber, station.Name, station.InfoId);
            _stationsToCreate.Add(newStation);
            _existingStations.Add(newStation);
        }
    }
    
    private void ExtractMissingPassengerInformation(RouteSnapshot routeSnapshot)
    {
        var unknownPassengerInformation = routeSnapshot.GetUnknownPassengerInformation(_existingPassengerInformation);
        
        foreach (var passengerInformation in unknownPassengerInformation)
        {
            var newInfoMessage = PassengerInformation.FoundNew(passengerInformation.OriginalText, passengerInformation.Priority);
            _passengerInformationToCreate.Add(newInfoMessage);
            _existingPassengerInformation.Add(newInfoMessage);
        }
    }
}