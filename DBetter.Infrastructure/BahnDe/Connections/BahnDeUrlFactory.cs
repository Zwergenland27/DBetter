using System.Web;
using DBetter.Application;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Connections;

public class BahnDeUrlFactory(SuggestionRequest suggestionRequest)
{
    private const string BaseUrl = "https://www.bahn.de/buchung/fahrplan/suche#";
    
    private Dictionary<string, string> _parameters = [];

    public string ForConnection(string ctxRecon)
    {
        var parameters = Build();
        return $"{BaseUrl}{parameters}&gh={ctxRecon}&cbs=true";
    }

    private string Build()
    {
        SetComfortClass();
        SetPassengers();
        SetOrigin();
        SetDestination();
    
        if (suggestionRequest.DepartureTime is not null)
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
        _parameters["kl"] = Klasse.GetUrlAliasFromComfortClass(suggestionRequest.ComfortClass);
    }

    private void SetPassengers()
    {
        _parameters["r"] = string.Join(',', suggestionRequest.Passengers.Select(ToReisender));

        if (suggestionRequest.AnyBikes)
        {
            var numberOfBikes = suggestionRequest.Passengers.Sum(passenger => passenger.Bikes);
            _parameters["r"] += $",{ReisenderTyp.GetBikeUrlId()}:{ArtErmaessigung.NoneUrlId}:{numberOfBikes}";
        }

        if (suggestionRequest.AnyDogs)
        {
            var numberOfDogs = suggestionRequest.Passengers.Sum(passenger => passenger.Dogs);
            _parameters["r"] += $",{ReisenderTyp.GetDogUrlId()}:{ArtErmaessigung.NoneUrlId}:{numberOfDogs}";
        }
    }
    
    private static string ToReisender(SuggestionRequestPassenger passenger)
    {
        var type = ReisenderTyp.GetUrlIdFromAge(passenger.Age);
        
        var count = 1;
        
        return $"{type}:{GenerateUriDiscounts(passenger.Discounts)}:{count}:{passenger.Age}";
    }

    private static string GenerateUriDiscounts(List<PassengerDiscount> discounts)
    {
        if (!discounts.Any())
        {
            return $"{ArtErmaessigung.None}:{KlasseErmaessigung.GetAliasFromComfortClass(ComfortClass.Unknown)}";
        }
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

    private static string GenerateUriDiscount(PassengerDiscount discount)
    {
        var type = ArtErmaessigung.GetUrlIdFromType(discount.Type);
        var comfortClass = KlasseErmaessigung.GetAliasFromComfortClass(discount.ComfortClass);
        return $"{type}:{comfortClass}";
    }

    private void SetOrigin()
    {
        _parameters["sot"] = "ST";
        _parameters["so"] = suggestionRequest.Route.Origin.Name.Value;
        _parameters["soid"] = suggestionRequest.Route.Origin.EvaNumber.AsFuzzy();
    }

    private void SetDestination()
    {
        _parameters["zot"] = "ST";
        _parameters["zo"] = suggestionRequest.Route.Destination.Name.Value;
        _parameters["zoid"] = suggestionRequest.Route.Destination.EvaNumber.AsFuzzy();
    }

    private void SetDepartureTime()
    {
        _parameters["hza"] = "D";
        _parameters["hd"] = suggestionRequest.DepartureTime!.Value.ToBahnTime();
    }

    private void SetArrivalTime()
    {
        _parameters["hza"] = "A";
        _parameters["hd"] = suggestionRequest.ArrivalTime!.Value.ToBahnTime();
    }

    private void SetStopovers()
    {
        _parameters["vm"] = GenerateAllowedMeansOfTransport(suggestionRequest.Route.MeansOfTransportFirstSection);

        var uriStopovers = "";
        if (suggestionRequest.Route.FirstStopover is not null)
        {
            uriStopovers += (GenerateUriStopover(suggestionRequest.Route.FirstStopover));
        }

        if (suggestionRequest.Route.SecondStopover is not null)
        {
            uriStopovers += ",";
            uriStopovers += GenerateUriStopover(suggestionRequest.Route.SecondStopover);
        }
        
        _parameters["hz"] = $"[{uriStopovers}]";
    }

    private string GenerateUriStopover(SuggestionRequestStopover stopover)
    {
        var evaNumber = stopover.Station.EvaNumber.AsFuzzy();
        var name = stopover.Station.Name.Value;
        var stayDuration = stopover.StayDuration;
        var meansOfTransport = GenerateAllowedMeansOfTransport(stopover.MeansOfTransportNextSection);
        return
            $"[\"{evaNumber}\",\"{name}\",{stayDuration},\"{meansOfTransport}\"]";
    }

    private static string GenerateAllowedMeansOfTransport(MeansOfTransport meansOfTransport)
    {
        return string.Join(',', meansOfTransport
            .AsList()
            .SelectMany(Produktgattung.GetUrlIdsFromTransportCategory));
    }

    private void SetSeatReservationOnly()
    {
        _parameters["ar"] = suggestionRequest.SeatReservationOnly ? "true" : "false";
    }

    private void SetFastConnectionsOnly()
    {
        _parameters["s"] = suggestionRequest.FastConnectionsOnly ? "true" : "false";
    }

    private void SetDirectConnectionsOnly()
    {
        _parameters["d"] = suggestionRequest.Route.MaxTransfers.Value is 0 ? "true" : "false";
    }

    private void SetBikeCarriage()
    {
        _parameters["fm"] = suggestionRequest.AnyBikes ? "true" : "false";
    }

    private void SetMinTransferTime()
    {
        _parameters["mud"] = suggestionRequest.Route.MinTransferTime.Value.ToString();
    }

    private void SetShowBestPrices()
    {
        _parameters["bp"] = "false";
    }

    private void SetDeutschlandTicketOnly()
    {
        _parameters["dlt"] = suggestionRequest.DeutschlandTicketConnectionsOnly ? "true" : "false";
    }
    
    private void SetDeutschlandTicketExists()
    {
        _parameters["dltv"] = suggestionRequest.AllPassengersOwnDeutschlandTicket ? "true" : "false";
    }
}