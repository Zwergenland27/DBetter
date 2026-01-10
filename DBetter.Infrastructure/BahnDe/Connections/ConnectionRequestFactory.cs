using System.Diagnostics;
using DBetter.Application;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Domain.ConnectionRequests.Entities;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public interface IConnectionRequestFactory
{
    ReiseAnfrage Build();
    TeilstreckeAnfrage BuildWithIncreasedTransferTime(string ctxRecon);
}

public class ConnectionRequestFactory
    : IConnectionRequestFactory
{
    private record IncreasedTransferTimeParameters(string CtxRecon);

    private IncreasedTransferTimeParameters _increasedTransferTimeParameters = null!;


    private SuggestionRequest _suggestionRequest;

    private ConnectionRequestFactory(SuggestionRequest suggestionRequest)
    {
        _suggestionRequest = suggestionRequest;
    }

    public static IConnectionRequestFactory FromRequest(SuggestionRequest request)
    {
        return new ConnectionRequestFactory(request);
    }
    
    public ReiseAnfrage Build()
    {
        return new()
        {
            AbfahrtsHalt = _suggestionRequest.Route.Origin.EvaNumber.Value,
            AnkunftsHalt = _suggestionRequest.Route.Destination.EvaNumber.Value,
            AnfrageZeitpunkt = GetRequestTime(),
            Klasse = Klasse.GetAliasFromComfortClass(_suggestionRequest.ComfortClass),
            AnkunftSuche = GetSearchMode(),
            Produktgattungen = ToProduktgattungen(_suggestionRequest.Route.MeansOfTransportFirstSection),
            Reisende = GetPassengers(),
            MaxUmstiege = _suggestionRequest.Route.MaxTransfers.Value,
            MinUmstiegszeit = _suggestionRequest.Route.MinTransferTime.Value,
            SchnelleVerbindungen = _suggestionRequest.FastConnectionsOnly,
            SitzplatzOnly = _suggestionRequest.SeatReservationOnly,
            BikeCarriage = _suggestionRequest.AnyBikes,
            DeutschlandTicketVorhanden = _suggestionRequest.AllPassengersOwnDeutschlandTicket,
            NurDeutschlandTicketVerbindungen = _suggestionRequest.DeutschlandTicketConnectionsOnly,
            Zwischenhalte = GetStopovers(),
            PagingReference = _suggestionRequest.PaginationToken,
        };;
    }

    public TeilstreckeAnfrage BuildWithIncreasedTransferTime(string ctxRecon)
    {
        _increasedTransferTimeParameters = new IncreasedTransferTimeParameters(ctxRecon);
        throw new NotImplementedException();
    }
    
    private string GetRequestTime()
    {
        if (_suggestionRequest.DepartureTime is not null)
        {
            return _suggestionRequest.DepartureTime.Value.ToBahnTime();
        }

        if (_suggestionRequest.ArrivalTime is not null)
        {
            return _suggestionRequest.ArrivalTime.Value.ToBahnTime();
        }

        throw new UnreachableException("DepartureTime and ArrivalTime of request object are not set!");
    }
    
    private string GetSearchMode()
    {
        if (_suggestionRequest.DepartureTime is not null)
        {
            return AnkunftSuche.Departure;
        }

        if (_suggestionRequest.ArrivalTime is not null)
        {
            return AnkunftSuche.Arrival;
        }
        
        throw new UnreachableException("DepartureTime and ArrivalTime of request object are not set!");
    }

    private List<Reisender> GetPassengers()
    {
        if (!_suggestionRequest.Passengers.Any())
        {
            return
            [
                Reisender.Default
            ];
        }
        
        var bikes = _suggestionRequest.Passengers.Sum(passenger => passenger.Bikes);
        var dogs =  _suggestionRequest.Passengers.Sum(passenger => passenger.Dogs);
        
        var passengers = _suggestionRequest.Passengers
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
    
    private static Reisender ToReisender(SuggestionRequestPassenger passenger)
    {
        return new Reisender
        {
            Alter = [passenger.Age.ToString()],
            Anzahl = 1,
            Ermaessigungen = ToErmaessigungen(passenger.Discounts),
            Typ = ReisenderTyp.GetTypeFromAge(passenger.Age)
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

        if (_suggestionRequest.Route.FirstStopover is not null)
        {
            var stopover =  _suggestionRequest.Route.FirstStopover;
            var meansOfTransport = stopover.MeansOfTransportNextSection;

            if (meansOfTransport is null)
                throw new UnreachableException("Stopover is set without means of transport for next section");

            stopovers.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = stopover.StayDuration > 0 ? stopover.StayDuration : null,
                Id = stopover.Station.EvaNumber.AsFuzzy(),
                VerkehrsmittelOfNextAbschnitt = ToProduktgattungen(meansOfTransport)
            });
        }
        
        if (_suggestionRequest.Route.SecondStopover is not null)
        {
            var stopover =  _suggestionRequest.Route.SecondStopover;
            var meansOfTransport = stopover.MeansOfTransportNextSection;
            
            if(meansOfTransport is null) throw new UnreachableException("Stopover is set without means of transport for next section");
            
            stopovers.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = stopover.StayDuration > 0 ? stopover.StayDuration : null,
                Id = stopover.Station.EvaNumber.AsFuzzy(),
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
}