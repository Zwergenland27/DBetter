using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Entities;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Routes.Entities;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Connections;

public interface IConnectionsRequestIdSelectionStage {
    IConnectionsExistingRoutesSelectionStage WithRequestId(ConnectionRequestId id);
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
    IConnectionExistingRoutesSelectionStage WithRequestId(ConnectionRequestId id);
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
    IConnectionRequestIdSelectionStage, IConnectionExistingRoutesSelectionStage, IConnectionExistingStationsSelectionStage, IConnectionFactory
{
    private List<Station> _stationsToCreate = [];
    private List<RouteEntity> _routesToCreate = [];
    private List<ConnectionEntity> _connectionsToCreate = [];
    
    private Fahrplan? _fahrplan;
    private Teilstrecke? _teilstrecke;

    private ConnectionRequestId? _requestId;

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

    IConnectionsExistingRoutesSelectionStage IConnectionsRequestIdSelectionStage.WithRequestId(ConnectionRequestId id)
    {
        _requestId = id;
        return this;
    }

    IConnectionExistingRoutesSelectionStage IConnectionRequestIdSelectionStage.WithRequestId(ConnectionRequestId id)
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
            if (abschnitt.Verkehrsmittel.Typ is VerkehrsmittelTyp.WALK)
            {
                segments.Add(new WalkingSegmentDto
                {
                    Distance = abschnitt.Distanz!.Value,
                    Duration = abschnitt.AbschnittsDauer
                });
                
                continue;
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
                verbindungsabschnitt.Verkehrsmittel.MittelText!, 
                verbindungsabschnitt.Verkehrsmittel.LangText!)[0];
            
            route = new RouteEntity(
                RouteId.CreateNew(),
                journeyId,
                routeInformation);
            
            _routesToCreate.Add(route);
            _existingRoutes.Add(journeyId, route);
        }

        var destinationStationExists = _existingStations.TryGetValue(destinationEvaNumber, out var station);
        if(!destinationStationExists){
            route!.DestinationStationMissing();
        }
        
        return new TransportSegmentDto
        {
            RouteId = route!.Id.Value.ToString(),
            Demand = verbindungsabschnitt.GetDemand().ToDto(),
            Destination = station?.Name.Value,
            Product = route.Information.Product.ToString(),
            Number = route.Information.GetBookingRelevantNumber(),
            BikeCarriage = RouteInformationFactory.CreateBikeCarriageInformation(
                verbindungsabschnitt.Verkehrsmittel!.Zugattribute,
                verbindungsabschnitt.HimMeldungen,
                verbindungsabschnitt.Halte)
                .ToDto(),
            Catering = RouteInformationFactory.CreateCateringInformation(
                verbindungsabschnitt.Verkehrsmittel!.Zugattribute,
                route.Information.Product,
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
            DepartureTime = halt.GetDepartureTime().ToDto(),
            ArrivalTime = halt.GetArrivalTime().ToDto(),
            Demand = halt.GetDemand().ToDto(),
            Name = station!.Name.Value,
            Platform = halt.GetPlatform().ToDto(),
            StopIndex = halt.GetStopIndex().Value
        };
    }
}