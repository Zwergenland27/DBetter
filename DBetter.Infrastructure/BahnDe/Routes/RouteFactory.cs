using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Routes;


public interface IExistingInformationSelectionStage
{
    IExistingStationsSelectionStage WithExistingInformation(RouteEntity route);
}

public interface IExistingStationsSelectionStage
{
    RouteFactory WithExistingStations(Dictionary<EvaNumber, Station> existingStations);
}

public class RouteFactory : IExistingInformationSelectionStage, IExistingStationsSelectionStage
{
    private List<Station> _stationsToCreate = [];

    private Fahrt _fahrt;

    private RouteEntity? _route = null!;

    private Dictionary<EvaNumber, Station> _existingStations = [];

    private RouteFactory(Fahrt fahrt)
    {
        _fahrt = fahrt;
    }

    public IReadOnlyList<Station> StationsToCreate => _stationsToCreate.AsReadOnly();

    public RouteDto RouteDto { get; private set; } = null!;

    public static IExistingInformationSelectionStage Create(Fahrt fahrt)
    {
        return new RouteFactory(fahrt);
    }

    public IExistingStationsSelectionStage WithExistingInformation(RouteEntity route)
    {
        _route = route;
        return this;
    }


    public RouteFactory WithExistingStations(Dictionary<EvaNumber, Station> existingStations)
    {
        _existingStations = existingStations;

        var serviceNummer = _fahrt.Halte[0].Nummer;

        RouteDto = new RouteDto
        {
            RouteId = _route!.Id.Value.ToString(),
            Product = _route!.Information.Product.ToString(),
            Stops = GetStops(_fahrt),
            Operator = RouteInformationFactory.GetOperator(_fahrt.Zugattribute),
            LineNumber = _route!.Information.LineNumber?.ToString(),
            ServiceNumber = RouteInformationFactory.GetServiceNumber(serviceNummer)?.ToString(),
            BikeCarriage = RouteInformationFactory.CreateBikeCarriageInformation(_fahrt.Zugattribute, _fahrt.HimMeldungen, _fahrt.Halte).ToDto(),
            Catering = RouteInformationFactory.CreateCateringInformation(_fahrt.Zugattribute, _route!.Information.Product, _fahrt.Halte).ToDto()
        };

        return this;
    }

    private List<StopDto> GetStops(Fahrt fahrt){
        return fahrt.Halte
            .Select(ToDto)
            .ToList();
    }

    private StopDto ToDto(Halt halt){
        var evaNumber = EvaNumber.Create(halt.ExtId).Value;
        var stationExists = _existingStations.TryGetValue(evaNumber, out var station);
        if(!stationExists){
            station = new Station(
                StationId.CreateNew(),
                evaNumber,
                halt.GetStationName(),
                null,
                halt.GetStationInfoId()
            );

            _stationsToCreate.Add(station);
            _existingStations.Add(evaNumber, station);
        }

        
        return new StopDto {
            Id = station!.Id.Value.ToString(),
            DepartureTime = halt.GetDepartureTime().ToDto(),
            ArrivalTime = halt.GetArrivalTime().ToDto(),
            Demand = halt.GetDemand().ToDto(),
            Name = station!.Name.Value,
            Platform = halt.GetPlatform().ToDto(),
            StopIndex = halt.GetStopIndex().Value
        };
    }
}