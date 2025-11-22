using System.Diagnostics;
using DBetter.Application;
using DBetter.Application.Requests;
using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public interface IRequestFactoryStationSelectionStage
{
    IRequestSelectionStage WithStations(List<Station> stations);
}

public interface IRequestSelectionStage
{
    INormalRequest WithSuggestionMode(SuggestionMode suggestionMode);

    IIncreaseTransferTimeRequest ByIncreasingTransferTime(string ctxRecon);
}

public interface INormalRequest
{
    ReiseAnfrage RequestObject { get; }
}

public interface IIncreaseTransferTimeRequest
{
    TeilstreckeAnfrage IncreasedTransferTimeRequestObject { get; }
}

public class RequestFactory : IRequestFactoryStationSelectionStage, IRequestSelectionStage, INormalRequest, IIncreaseTransferTimeRequest
{
    private record IncreasedTransferTimeParameters(string CtxRecon);
    
    private ConnectionRequest _request;
    private List<Station> _stations = [];
    private SuggestionMode _suggestionMode = SuggestionMode.Normal;
    private IncreasedTransferTimeParameters _increasedTransferTimeParameters = null!;
    
    public ReiseAnfrage RequestObject { get; private set; } = null!;

    public TeilstreckeAnfrage IncreasedTransferTimeRequestObject { get; private set; } = null!;

    private RequestFactory(ConnectionRequest request)
    {
        _request = request;
    }

    public static IRequestFactoryStationSelectionStage CreateFrom(ConnectionRequest request)
    {
        return new RequestFactory(request);
    }

    public IRequestSelectionStage WithStations(List<Station> stations)
    {
        _stations = stations;
        return this;
    }

    public INormalRequest WithSuggestionMode(SuggestionMode suggestionMode)
    {
        _suggestionMode = suggestionMode;
        RequestObject = new()
        {
            AbfahrtsHalt = GetDepartureId(),
            AnkunftsHalt = GetDestinationId(),
            AnfrageZeitpunkt = GetRequestTime(),
            Klasse = Klasse.GetAliasFromComfortClass(_request.ComfortClass),
            AnkunftSuche = GetSearchMode(),
            Produktgattungen = ToProduktgattungen(_request.Route.MeansOfTransportFirstSection),
            Reisende = GetPassengers(),
            MaxUmstiege = _request.Route.MaxTransfers.Value,
            MinUmstiegszeit = _request.Route.MinTransferTime.Value,
            SchnelleVerbindungen = false,
            SitzplatzOnly = false,
            BikeCarriage = _request.AnyBikes,
            DeutschlandTicketVorhanden = _request.AllPassengersOwnDeutschlandTicket,
            NurDeutschlandTicketVerbindungen = false,
            Zwischenhalte = GetStopovers(),
            PagingReference = GetPagination(),
        };
        return this;
    }

    public IIncreaseTransferTimeRequest ByIncreasingTransferTime(string ctxRecon)
    {
        _increasedTransferTimeParameters = new  IncreasedTransferTimeParameters(ctxRecon);
        throw new NotImplementedException();
        return this;
    }

    private string GetDepartureId()
    {
        return _stations
            .First(s => s.Id == _request.Route.OriginStationId)
            .EvaNumber.AsFuzzy();
    }

    private string GetDestinationId()
    {
        return _stations
            .First(s => s.Id == _request.Route.DestinationStationId)
            .EvaNumber.AsFuzzy();
    }
    
    private string GetRequestTime()
    {
        if (_request.DepartureTime is not null)
        {
            return _request.DepartureTime.Value.ToBahnTime();
        }

        if (_request.ArrivalTime is not null)
        {
            return _request.ArrivalTime.Value.ToBahnTime();
        }

        throw new UnreachableException("DepartureTime and ArrivalTime of request object are not set!");
    }
    
    private string GetSearchMode()
    {
        if (_request.DepartureTime is not null)
        {
            return AnkunftSuche.Departure;
        }

        if (_request.ArrivalTime is not null)
        {
            return AnkunftSuche.Arrival;
        }
        
        throw new UnreachableException("DepartureTime and ArrivalTime of request object are not set!");
    }

    private List<Reisender> GetPassengers()
    {
        if (!_request.Passengers.Any())
        {
            return
            [
                Reisender.Default
            ];
        }
        
        var bikes = _request.Passengers.Sum(passenger => passenger.Bikes);
        var dogs =  _request.Passengers.Sum(passenger => passenger.Dogs);
        
        var passengers = _request.Passengers
            .Select(ToReisender)
            .ToList();

        if (bikes > 0)
        {
            passengers.Add(Reisender.Bikes(bikes));
        }

        if (dogs > 0)
        {
            passengers.Add(Reisender.Dogs(dogs));
        }
        
        return passengers;
    }
    
    private static Reisender ToReisender(Passenger passenger)
    {
        if(passenger.Age is null) throw new NotImplementedException("Passenger with birthday is not supported yet");

        var age = passenger.Age.Value;

        return new Reisender
        {
            Alter = [age.ToString()],
            Anzahl = 1,
            Ermaessigungen = ToErmaessigungen(passenger.Discounts),
            Typ = ReisenderTyp.GetTypeFromAge(age)
        };
    }
    
    private static List<Ermaessigung> ToErmaessigungen(IEnumerable<PassengerDiscount> discounts)
    {
        var ermaessigungen = discounts
            .Where(d => d.Type is not DiscountType.DeutschlandTicket)
            .Select(Ermaessigung.Create).ToList();

        return ermaessigungen.Any() ? ermaessigungen : [Ermaessigung.None()];
    }

    private List<Zwischenhalt> GetStopovers()
    {
        var stopovers = new List<Zwischenhalt>();

        if (_request.Route.FirstStopover is not null)
        {
            var stopover =  _request.Route.FirstStopover;
            var meansOfTransport = stopover.MeansOfTransportNextSection;

            if (meansOfTransport is null)
                throw new UnreachableException("Stopover is set without means of transport for next section");

            stopovers.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = stopover.LengthOfStay > 0 ? stopover.LengthOfStay : null,
                Id = _stations.First(s => s.Id == stopover.StationId).EvaNumber.AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = ToProduktgattungen(meansOfTransport)
            });
        }
        
        if (_request.Route.SecondStopover is not null)
        {
            var stopover =  _request.Route.SecondStopover;
            var meansOfTransport = stopover.MeansOfTransportNextSection;
            
            if(meansOfTransport is null) throw new UnreachableException("Stopover is set without means of transport for next section");
            
            stopovers.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = stopover.LengthOfStay > 0 ? stopover.LengthOfStay : null,
                Id = _stations.First(s => s.Id == stopover.StationId).EvaNumber.AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = ToProduktgattungen(meansOfTransport)
            });
        }
        
        return stopovers;
    }
    
    private static List<string> ToProduktgattungen(MeansOfTransport meansOfTransport)
    {
        return meansOfTransport
            .AsList()
            .SelectMany(Produktgattung.GetAliasesFromTransportCategory)
            .ToList();
    }
    
    private string? GetPagination()
    {
        return _suggestionMode switch
        {
            SuggestionMode.Normal => null,
            SuggestionMode.Earlier => _request.EarlierReference?.Token,
            SuggestionMode.Later => _request.LaterReference?.Token,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}