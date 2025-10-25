using System.Web;
using DBetter.Domain.Stations;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public interface IStationSelectionStage
{
    BahnDeUrlFactory WithStations(List<Station> stations);
}

public class BahnDeUrlFactory : IStationSelectionStage
{
    private const string BaseUrl = "https://www.bahn.de/buchung/fahrplan/suche#";


    private ReiseAnfrage _request;
    private List<Station>? _stations;
    private Dictionary<string, string> _parameters = [];
    private string UrlParameters => string.Join("&", _parameters.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
    public string BahnDeUrl => BaseUrl + UrlParameters;

    private BahnDeUrlFactory(ReiseAnfrage request)
    {
        _request = request;
    }

    public static IStationSelectionStage FromRequest(ReiseAnfrage request)
    {
        return new BahnDeUrlFactory(request);
    }

    private void SetComfortClass()
    {
        _parameters["kl"] = _request.Klasse is Klasse.KLASSE_1 ? "1" : "2";
    }

    private void SetPassengers()
    {
        _parameters["r"] = string.Join(',', _request.Reisende.Select(GeneratePassenger));
    }
    
    private static string GeneratePassenger(Reisender passenger)
    {
        var type = passenger.Typ switch
        {
            ReisenderTyp.FAHRRAD => 3,
            ReisenderTyp.HUND => 14,
            ReisenderTyp.KLEINKIND => 8,
            ReisenderTyp.FAMILIENKIND => 11,
            ReisenderTyp.JUGENDLICHER => 9,
            ReisenderTyp.ERWACHSENER => 13,
            ReisenderTyp.SENIOR => 12,
            _ => throw new InvalidDataException()
        };

        var count = passenger.Anzahl;

        if (passenger.Alter.Any())
        {
            return $"{type}:{GenerateUriDiscounts(passenger.Ermaessigungen)}:{count}:{passenger.Alter[0]}";
        }
        
        return $"{type}:{GenerateUriDiscounts(passenger.Ermaessigungen)}:{count}";
    }

    private static string GenerateUriDiscounts(List<Ermaessigung> discounts)
    {
        var uriDiscounts = discounts
            .Select(GenerateUriDiscount)
            .ToList();
        
        if (discounts.Count == 1)
        {
            return uriDiscounts[0];
        }

        var otherDiscounts = string.Join('|', uriDiscounts.Skip(1));
        return $"{uriDiscounts[0]}[{otherDiscounts}]";
    }

    private static string GenerateUriDiscount(Ermaessigung discount)
    {
        var type = ArtErmaessigung.GetUrlIdFromAlias(discount.Art);
        return $"{type}:{discount.Klasse}";
    }

    private void SetOrigin()
    {
        if(_stations is null) throw new InvalidOperationException("Cannot set origin without .WithStations being called");
        
        _parameters["sot"] = "ST";
        var name = _stations
            .First(s => s.EvaNumber.AsFuzzy() == _request.AbfahrtsHalt)
            .Name.Value;
        _parameters["so"] = name;
        _parameters["soid"] = _request.AbfahrtsHalt;
    }

    private void SetDestination()
    {
        if(_stations is null) throw new InvalidOperationException("Cannot set destination without .WithStations being called");
        
        _parameters["zot"] = "ST";
        var name = _stations
            .First(s => s.EvaNumber.AsFuzzy() == _request.AnkunftsHalt)
            .Name.Value;
        _parameters["zo"] = name;
        _parameters["zoid"] = _request.AnkunftsHalt;
    }

    private void SetDepartureTime()
    {
        _parameters["hza"] = "D";
        _parameters["hd"] = _request.AnfrageZeitpunkt;
    }

    private void SetArrivalTime()
    {
        _parameters["hza"] = "A";
        _parameters["hd"] = _request.AnfrageZeitpunkt;
    }

    private void SetStopovers()
    {
        _parameters["vm"] = GenerateAllowedMeansOfTransport(_request.Produktgattungen);
        var uriStopovers = string.Join(',', _request.Zwischenhalte.Select(GenerateUriStopover));
        _parameters["hz"] = $"[{uriStopovers}]";
    }

    private string GenerateUriStopover(Zwischenhalt stopover)
    {
        if(_stations is null) throw new InvalidOperationException("Cannot generate uri stopover without .WithStations being called");
        
        var name = _stations
            .First(s => s.EvaNumber.AsFuzzy() == stopover.Id)
            .Name.Value;
        return
            $"[\"{stopover.Id}\",\"{name}\",{stopover.Aufenthaltsdauer ?? 0},\"{GenerateAllowedMeansOfTransport(stopover.VerkehrsmittelOfNextAbschnitt)}\"]";
    }

    private static string GenerateAllowedMeansOfTransport(List<Produktgattung> produktgattungen)
    {
        return string.Join(',', produktgattungen.Select(GenerateMeansOfTransportUriNumber));
    }

    private static string GenerateMeansOfTransportUriNumber(Produktgattung produktgattung)
    {
        return produktgattung switch
        {
            Produktgattung.ICE => "00",
            Produktgattung.EC_IC => "01",
            Produktgattung.IR => "02",
            Produktgattung.REGIONAL => "03",
            Produktgattung.SBAHN => "04",
            Produktgattung.BUS => "05",
            Produktgattung.SCHIFF => "06",
            Produktgattung.UBAHN => "07",
            Produktgattung.TRAM => "08",
            Produktgattung.ANRUFPFLICHTIG => "09",
            _ => throw new InvalidDataException()
        };
    }

    private void SetSeatReservationOnly()
    {
        _parameters["ar"] = _request.SitzplatzOnly ? "true" : "false";
    }

    private void SetFastConnectionsOnly()
    {
        _parameters["s"] = _request.SchnelleVerbindungen ? "true" : "false";
    }

    private void SetDirectConnectionsOnly()
    {
        _parameters["d"] = _request.MaxUmstiege == 0 ? "true" : "false";
    }

    private void SetBikeCarriage()
    {
        _parameters["fm"] = _request.BikeCarriage ? "true" : "false";
    }

    private void SetMinTransferTime()
    {
        _parameters["mud"] = _request.MinUmstiegszeit.ToString();
    }

    private void SetShowBestPrices()
    {
        _parameters["bp"] = "false";
    }

    private void SetDeutschlandTicketOnly()
    {
        _parameters["dlt"] = _request.NurDeutschlandTicketVerbindungen ? "true" : "false";
    }
    
    private void SetDeutschlandTicketExists()
    {
        _parameters["dltv"] = _request.DeutschlandTicketVorhanden ? "true" : "false";
    }

    public BahnDeUrlFactory WithStations(List<Station> stations)
    {
        _stations = stations;
        SetComfortClass();
        SetPassengers();
        SetOrigin();
        SetDestination();

        if (_request.AnkunftSuche is AnkunftSuche.ABFAHRT)
        {
            SetDepartureTime();
        }
        else
        {
            SetArrivalTime();
        }
        
        SetStopovers();
        SetSeatReservationOnly();
        SetFastConnectionsOnly();
        SetDirectConnectionsOnly();
        SetBikeCarriage();
        SetMinTransferTime();
        SetShowBestPrices();
        SetDeutschlandTicketExists();
        SetDeutschlandTicketOnly();
        return this;
    }

    public BahnDeUrlFactory ForConnection(string ctxRecon)
    {
        _parameters["gh"] = ctxRecon;
        _parameters["cbs"] = "true";
        return this;
    }
}