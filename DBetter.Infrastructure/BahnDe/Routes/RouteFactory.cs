using DBetter.Contracts.Routes.Queries.Get;
using DBetter.Contracts.Routes.Queries.Get.Results;
using DBetter.Domain.Routes;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Routes;


public interface IExistingInformationSelectionStage
{
    IExistingStationsSelectionStage WithExistingInformation(Route route);
}

public interface IExistingStationsSelectionStage
{
    RouteFactory WithExistingStations(Dictionary<EvaNumber, Station> existingStations);
}

public class RouteFactory : IExistingInformationSelectionStage, IExistingStationsSelectionStage
{
    private List<Station> _stationsToCreate = [];

    private Fahrt _fahrt;

    private Route? _route = null!;

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

    public IExistingStationsSelectionStage WithExistingInformation(Route route)
    {
        _route = route;
        return this;
    }


    public RouteFactory WithExistingStations(Dictionary<EvaNumber, Station> existingStations)
    {
        _existingStations = existingStations;

        var serviceNumber = _fahrt.Halte[0].Nummer;
        if (serviceNumber is not null && int.TryParse(serviceNumber, out var number))
        {
            _route!.UpdateServiceNumber(new ServiceNumber(number));
        }
        
        var bikeCarriageInformation = new BikeCarriageInformationFactory(_fahrt).ExtractInformation();
        _route!.TryUpdateBikeCarriageInformation(bikeCarriageInformation);
        
        var cateringInformation = new CateringInformationFactory(_fahrt).ExtractInformation();
        _route.TryUpdateCateringInformation(cateringInformation);
        
        RouteDto = new RouteDto
        {
            RouteId = _route!.Id.Value.ToString(),
            ServiceCategory = _route!.ServiceInformation.ProductClass,
            Stops = GetStops(_fahrt),
            Operator = _fahrt.Zugattribute.FirstOrDefault(a => a.Key is "BEF")?.Value,
            LineNumber = _route!.ServiceInformation.LineNumber?.ToString(),
            ServiceNumber = _route.ServiceInformation.ServiceNumber?.Value.ToString(),
            BikeCarriage = _route.BikeCarriage.ToDto(),
            Catering = _route.Catering.ToDto()
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
            station = Station.CreateFromRoute(evaNumber, halt.GetStationName(), halt.GetStationInfoId());

            _stationsToCreate.Add(station);
            _existingStations.Add(evaNumber, station);
        }

        
        return new StopDto {
            Id = station!.Id.Value.ToString(),
            DepartureTime = halt.GetDepartureTime().ToDto(),
            ArrivalTime = halt.GetArrivalTime().ToDto(),
            Demand = halt.GetDemand().ToDto(),
            Name = station.Name.NormalizedValue,
            Platform = halt.GetPlatform().ToDto(),
            IsAdditional = halt.IsAdditional(),
            IsCancelled = halt.IsCancelled(),
            IsEntryOnly = halt.IsEntryOnly(),
            IsExitOnly = halt.IsExitOnly(),
            StopIndex = halt.GetStopIndex().Value
        };
    }
}