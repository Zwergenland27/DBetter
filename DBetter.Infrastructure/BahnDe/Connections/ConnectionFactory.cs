using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class ConnectionFactory
{
    private List<Station> _stationsToCreate = [];
    private List<RouteEntity> _routesToCreate = [];
    
    private List<Verbindung> _verbindungen;
    private BahnDeUrlFactory _urlFactory;

    private Dictionary<BahnJourneyId, RouteEntity> _existingRoutes = [];
    private Dictionary<EvaNumber, Station> _existingStations = [];

    public ConnectionFactory(
        List<Verbindung> verbindungen,
        ReiseAnfrage originalRequest,
        Dictionary<BahnJourneyId, RouteEntity> existingRoutes,
        Dictionary<EvaNumber, Station> existingStations)
    {
        _verbindungen = verbindungen;
        _urlFactory = BahnDeUrlFactory
            .CreateFrom(originalRequest)
            .WithStations(existingStations.Select(d => d.Value).ToList());
        _existingRoutes = existingRoutes;
        _existingStations = existingStations;
    }
    
    
    public IReadOnlyList<Station> StationsToCreate => _stationsToCreate.AsReadOnly();
    public IReadOnlyList<RouteEntity> RoutesToCreate => _routesToCreate.AsReadOnly();

    public List<ConnectionResponse> GetConnections()
    {
        return _verbindungen
            .Select(ToDto)
            .ToList();
    }

    private ConnectionResponse ToDto(Verbindung verbindung)
    {
        OfferResponse? offer = null;
        if (verbindung.AngebotsPreis is not null)
        {
            offer = new OfferResponse
            {
                ComfortClass = Klasse.GetComfortClassFromAlias(verbindung.AngebotsPreisKlasse!).ToString(),
                Price = verbindung.AngebotsPreis.Betrag,
                Currency = verbindung.AngebotsPreis.Waehrung.ToCurrency().ToString(),
                Partial = verbindung.HasTeilpreis
            };
        }

        List<SegmentResponse> segments = [];
        var differentOrigin = false;

        foreach (var abschnitt in verbindung.VerbindungsAbschnitte)
        {
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK or VerkehrsmittelTyp.TRANSFER)
            {
                if (segments.Count > 0)
                {
                    segments.Add(new WalkingSegmentResponse
                    {
                        Distance = abschnitt.Distanz!.Value,
                        WalkDuration = abschnitt.AbschnittsDauer
                    });   
                }
                else
                {
                    differentOrigin = true;
                }
                
                continue;
            }
            
            if (segments.LastOrDefault() is TransportSegmentResponse)
            {
                segments.Add(new TransferSegmentResponse());
            }
            
            segments.Add(GetTransportSegment(abschnitt));
        }
        
        var differentDestination = false;
        var lastSegment = segments.LastOrDefault();
        if (lastSegment is WalkingSegmentResponse)
        {
            segments.Remove(lastSegment);
            differentDestination = true;
        }

        return new ConnectionResponse
            {
                Id = ConnectionId.CreateNew().Value.ToString(),
                DifferentOrigin = differentOrigin,
                DifferentDestination = differentDestination,
                BahnDeUrl = _urlFactory.ForConnection(verbindung.CtxRecon),
                Demand = verbindung.GetDemand().ToDto(),
                Segments = segments,
                Offer = offer
            };
    }

    private TransportSegmentResponse GetTransportSegment(VerbindungsAbschnitt verbindungsabschnitt)
    {
        var journeyId = new BahnJourneyId(verbindungsabschnitt.JourneyId!);

        var destinationEvaNumber = journeyId.GetDestinationEvaNumber();
        
        var routeExists = _existingRoutes.TryGetValue(journeyId, out var route);
        if (!routeExists)
        {
            var routeInformation = RouteInformationFactory.Create(
                Produktgattung.GetTransportCategoryFromAlias(verbindungsabschnitt.Verkehrsmittel.ProduktGattung!),
                verbindungsabschnitt.Verkehrsmittel.MittelText!, 
                verbindungsabschnitt.Verkehrsmittel.LangText!)[0];
            
            route = new RouteEntity(
                RouteId.CreateNew(),
                journeyId,
                routeInformation);
            
            _routesToCreate.Add(route);
            _existingRoutes.Add(journeyId, route);
        }

        var destination = verbindungsabschnitt.Verkehrsmittel.Richtung;
        if (destination is null || destination.All(char.IsUpper))
        {
            var destinationStationExists = _existingStations.TryGetValue(destinationEvaNumber, out var station);
            if(!destinationStationExists){
                route!.DestinationStationMissing();
            }

            destination = station?.Name.NormalizedValue;
        }
        
        return new TransportSegmentResponse
        {
            DepartureTime = verbindungsabschnitt.GetDepartureTime().ToDto()!,
            ArrivalTime = verbindungsabschnitt.GetArrivalTime().ToDto()!,
            RouteId = route!.Id.Value.ToString(),
            Demand = verbindungsabschnitt.GetDemand().ToDto(),
            Operator = RouteInformationFactory.GetOperator(verbindungsabschnitt.Verkehrsmittel!.Zugattribute),
            Destination = destination,
            TransportCategory = route.Information.TransportCategory.ToString(),
            Line = route.Information.GetLine(),
            BikeCarriage = RouteInformationFactory.CreateBikeCarriageInformation(
                verbindungsabschnitt.Verkehrsmittel!.Zugattribute,
                verbindungsabschnitt.HimMeldungen,
                verbindungsabschnitt.Halte)
                .ToDto(),
            Catering = RouteInformationFactory.CreateCateringInformation(
                    verbindungsabschnitt.Verkehrsmittel.Zugattribute,
                verbindungsabschnitt.Halte
                )
                .ToDto(),
            Stops = GetStops(verbindungsabschnitt)
        };
    }

    private List<StopResponse> GetStops(VerbindungsAbschnitt verbindungsabschnitt){
        return verbindungsabschnitt.Halte
            .Select(ToDto)
            .ToList();
    }

    private StopResponse ToDto(DTOs.Halt halt){
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

        return new StopResponse {
            Id = station!.Id.Value.ToString(),
            DepartureTime = halt.GetDepartureTime().ToDto(),
            ArrivalTime = halt.GetArrivalTime().ToDto(),
            Demand = halt.GetDemand().ToDto(),
            Name = station.Name.NormalizedValue,
            Ril100 = station.Ril100?.Value,
            Platform = halt.GetPlatform().ToDto(),
            IsAdditional = halt.IsAdditional(),
            IsCancelled = halt.IsCancelled(),
            IsEntryOnly = halt.IsEntryOnly(),
            IsExitOnly = halt.IsExitOnly(),
            StopIndex = halt.GetStopIndex().Value
        };
    }
}