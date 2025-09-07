using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Contracts.Shared.DTOs;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.Repositories;

namespace DBetter.Infrastructure.BahnDe.Connections;

public interface IConnectionsRequestIdSelectionStage {
    IConnectionsExistingRoutesSelectionStage WithRequest(ConnectionRequestId id, ReiseAnfrage anfrage);
}

public interface IConnectionsExistingRoutesSelectionStage
{
    IConnectionsExistingStationsSelectionStage WithExistingRoutes(Dictionary<JourneyId, RouteEntity> existingRoutes);
}

public interface IConnectionsExistingStationsSelectionStage
{
    IConnectionsFactory WithExistingStations(Dictionary<EvaNumber, Station> existingStations);
}

public interface IConnectionsFactory {
    IReadOnlyList<Station> StationsToCreate { get; }

    IReadOnlyList<RouteEntity> RoutesToCreate { get; }

    IReadOnlyList<ConnectionEntity> ConnectionsToCreate { get; }

    public ConnectionSuggestionsDto SuggestionsDto { get; }
}

public interface IConnectionRequestIdSelectionStage {
    IConnectionExistingRoutesSelectionStage WithRequest(ConnectionRequestId id);
}

public interface IConnectionExistingRoutesSelectionStage
{
    IConnectionExistingStationsSelectionStage WithExistingRoutes(Dictionary<JourneyId, RouteEntity> existingRoutes);
}

public interface IConnectionExistingStationsSelectionStage
{
    IConnectionFactory WithExistingStations(Dictionary<EvaNumber, Station> existingStations);
}

public interface IConnectionFactory {
    IReadOnlyList<Station> StationsToCreate { get; }

    IReadOnlyList<RouteEntity> RoutesToCreate { get; }

    IReadOnlyList<ConnectionEntity> ConnectionsToCreate { get; }

    public ConnectionDto ConnectionDto { get; }
}

public class ConnectionFactory :
    IConnectionsRequestIdSelectionStage, IConnectionsExistingRoutesSelectionStage, IConnectionsExistingStationsSelectionStage, IConnectionsFactory,
    IConnectionRequestIdSelectionStage, IConnectionExistingRoutesSelectionStage, IConnectionExistingStationsSelectionStage,  IConnectionFactory
{
    private List<Station> _stationsToCreate = [];
    private List<RouteEntity> _routesToCreate = [];
    private List<ConnectionEntity> _connectionsToCreate = [];
    
    private Fahrplan? _fahrplan;
    private Teilstrecke? _teilstrecke;

    private ConnectionRequestId? _requestId;
    private ReiseAnfrage? _anfrage;

    private Dictionary<JourneyId, RouteEntity> _existingRoutes = [];
    private Dictionary<EvaNumber, Station> _existingStations = [];

    private ConnectionFactory(Fahrplan fahrplan)
    {
        _fahrplan = fahrplan;
    }

    private ConnectionFactory(Teilstrecke teilstrecke)
    {
        _teilstrecke = teilstrecke;
    }
    
    public IReadOnlyList<Station> StationsToCreate => _stationsToCreate.AsReadOnly();
    public IReadOnlyList<RouteEntity> RoutesToCreate => _routesToCreate.AsReadOnly();
    public IReadOnlyList<ConnectionEntity> ConnectionsToCreate => _connectionsToCreate.AsReadOnly();

    public ConnectionSuggestionsDto SuggestionsDto { get; private set; } = null!;

    public ConnectionDto ConnectionDto {get; private set; } = null!;
    
    public static IConnectionsRequestIdSelectionStage CreateFrom(Fahrplan fahrplan)
    {
        return new ConnectionFactory(fahrplan);
    }

    public static IConnectionRequestIdSelectionStage CreateFrom(Teilstrecke teilstrecke){
        return new ConnectionFactory(teilstrecke);
    }

    IConnectionsExistingRoutesSelectionStage IConnectionsRequestIdSelectionStage.WithRequest(ConnectionRequestId id, ReiseAnfrage anfrage)
    {
        _requestId = id;
        _anfrage = anfrage;
        return this;
    }

    IConnectionExistingRoutesSelectionStage IConnectionRequestIdSelectionStage.WithRequest(ConnectionRequestId id)
    {
        _requestId = id;
        return this;
    }

    IConnectionsExistingStationsSelectionStage IConnectionsExistingRoutesSelectionStage.WithExistingRoutes(Dictionary<JourneyId, RouteEntity> existingRoutes)
    {
        _existingRoutes = existingRoutes;
        return this;
    }

    IConnectionExistingStationsSelectionStage IConnectionExistingRoutesSelectionStage.WithExistingRoutes(Dictionary<JourneyId, RouteEntity> existingRoutes){
        _existingRoutes = existingRoutes;
        return this;
    }

    IConnectionsFactory IConnectionsExistingStationsSelectionStage.WithExistingStations(Dictionary<EvaNumber, Station> existingStations)
    {
        _existingStations = existingStations;
        
        SuggestionsDto = new ConnectionSuggestionsDto
        {
            RequestId = _requestId!.Value.ToString(),
            Connections = GetConnections(),
            PageEarlier = _fahrplan!.VerbindungReference.Earlier,
            PageLater = _fahrplan!.VerbindungReference.Later,
        };

        return this;
    }

    IConnectionFactory IConnectionExistingStationsSelectionStage.WithExistingStations(Dictionary<EvaNumber, Station> existingStations){
        _existingStations = existingStations;
        
        ConnectionDto = ToDto(_teilstrecke!.Verbindung);
        
        return this;
    }

    private List<ConnectionDto> GetConnections()
    {
        return _fahrplan!.Verbindungen
            .Select(ToDto)
            .ToList();
    }

    private ConnectionDto ToDto(Verbindung verbindung)
    {
        OfferDto? offer = null;
        if (verbindung.AngebotsPreis is not null)
        {
            offer = new OfferDto
            {
                ComfortClass = verbindung.AngebotsPreisKlasse!.Value.ToComfortClass().ToString(),
                Price = verbindung.AngebotsPreis.Betrag,
                Currency = verbindung.AngebotsPreis.Waehrung.ToCurrency().ToString(),
                Partial = verbindung.HasTeilpreis
            };
        }

        List<SegmentDto> segments = [];

        foreach (var abschnitt in verbindung.VerbindungsAbschnitte)
        {
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK or VerkehrsmittelTyp.TRANSFER)
            {
                segments.Add(new WalkingSegmentDto
                {
                    Distance = abschnitt.Distanz!.Value,
                    WalkDuration = abschnitt.AbschnittsDauer
                });
                
                continue;
            }
            
            if (segments.LastOrDefault() is TransportSegmentDto)
            {
                segments.Add(new TransferSegmentDto());
            }
            
            segments.Add(GetTransportSegment(abschnitt));
        }

        var connectionId = ConnectionId.CreateNew();

        _connectionsToCreate.Add(new ConnectionEntity(
            connectionId,
            _requestId!,
            verbindung.CtxRecon
        ));

        return new ConnectionDto
            {
                Id = connectionId.Value.ToString(),
                BahnDeUrl = BahnDeUrlFactory.FromRequest(_anfrage!)
                    .WithStations(_existingStations.Select(kvp => kvp.Value).ToList())
                    .ForConnection(verbindung.CtxRecon)
                    .BahnDeUrl,
                Demand = verbindung.GetDemand().ToDto(),
                Segments = segments,
                Offer = offer
            };
    }

    private TransportSegmentDto GetTransportSegment(VerbindungsAbschnitt verbindungsabschnitt)
    {
        var journeyId = new JourneyId(verbindungsabschnitt.JourneyId!);

        var destinationEvaNumber = journeyId.GetDestinationEvaNumber();
        
        var routeExists = _existingRoutes.TryGetValue(journeyId, out var route);
        if (!routeExists)
        {
            var routeInformation = RouteInformationFactory.Create(
                verbindungsabschnitt.Verkehrsmittel.ProduktGattung!.Value,
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
        
        return new TransportSegmentDto
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

    private List<StopDto> GetStops(VerbindungsAbschnitt verbindungsabschnitt){
        return verbindungsabschnitt.Halte
            .Select(ToDto)
            .ToList();
    }

    private StopDto ToDto(DTOs.Halt halt){
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