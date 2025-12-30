using System.Web;
using DBetter.Domain.Stations;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public interface IBahnDeUrlFactoryStationSelectionStage
{
    BahnDeUrlFactory WithStations(List<Station> stations);
}

public class BahnDeUrlFactory : IBahnDeUrlFactoryStationSelectionStage
{
    private const string BaseUrl = "https://www.bahn.de/buchung/fahrplan/suche#";


    private readonly ReiseAnfrage _request;
    private List<Station> _stations;
    
    private Dictionary<string, string> _parameters = [];
    public string GenericUrl { get; private set; }

    private BahnDeUrlFactory(ReiseAnfrage request)
    {
        _request = request;
    }

    public static IBahnDeUrlFactoryStationSelectionStage CreateFrom(ReiseAnfrage request)
    {
        return new BahnDeUrlFactory(request);
    }
    
    public BahnDeUrlFactory WithStations(List<Station> stations)
    {
        _stations = stations;
        GenericUrl = $"{BaseUrl}/{Build()}";
        return this;
    }

    public string ForConnection(string ctxRecon)
    {
        return $"{GenericUrl}&gh={ctxRecon}&cbs=true";
    }

    private string Build()
    {
        SetComfortClass();
        SetPassengers();
        SetOrigin();
        SetDestination();
    
        if (_request.AnkunftSuche == AnkunftSuche.Departure)
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
        
        return string.Join("&", _parameters.Select(kvp => $"{kvp.Key}={HttpUtility.UrlEncode(kvp.Value)}"));
    }

    private void SetComfortClass()
    {
        _parameters["kl"] = Klasse.GetUrlAliasFromAlias(_request.Klasse);
    }

    private void SetPassengers()
    {
        _parameters["r"] = string.Join(',', _request.Reisende.Select(ToReisender));
    }
    
    private static string ToReisender(Reisender passenger)
    {
        var type = ReisenderTyp.GetUrlIdFromType(passenger.Typ);

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

    private static string GenerateAllowedMeansOfTransport(List<string> produktgattungen)
    {
        return string.Join(',', produktgattungen.SelectMany(Produktgattung.GetUrlIdsFromAlias));
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
}