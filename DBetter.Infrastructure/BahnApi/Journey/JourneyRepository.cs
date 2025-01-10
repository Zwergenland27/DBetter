using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using DBetter.Contracts.Journeys.DTOs;
using DBetter.Contracts.Journeys.Parameters;
using DBetter.Domain.Users.ValueObjects;
using DBetter.Infrastructure.BahnApi.Journey.Parameters;
using DBetter.Infrastructure.BahnApi.Journey.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DBetter.Infrastructure.BahnApi.Journey;

public class JourneyRepository(HttpClient http)
{
    public async Task<ConnectionsDto?> GetRoutes(RequestParameters parameters, string? page)
    {
        var request = new ReiseAnfrage
        {
            AbfahrtsHalt = parameters.Route.Origin!.Id,
            AnkunftsHalt = parameters.Route.Destination!.Id,
            AnfrageZeitpunkt = parameters.Time.ConvertToBahnTime(),
            Klasse = parameters.Options.Class == "First" ? "KLASSE_1" : "KLASSE_2",
            AnkunftSuche = parameters.TimeType == "Departure" ? "ABFAHRT" : "ANKUNFT",
            Produktgattungen = parameters.Route.RouteOptions[0].GetAllowedTransport(),
            Reisende = generatePassengers(parameters.Passengers),
            SchnelleVerbindungen = false,
            SitzplatzOnly = false,
            BikeCarriage = false,
            ReservierungsKontingenteVorhanden = false,
            NurDeutschlandTicketVerbindungen = false,
            DeutschlandTicketVorhanden = false,
            Zwischenhalte = parameters.Route.ToZwischenhalte(),
            MinUmstiegszeit = parameters.Options.MinTransferTime,
            MaxUmstiege = parameters.Options.MaxTransfers,
            PagingReference = page
        };

        var response = await http.PostAsJsonAsync("angebote/fahrplan", request);
        if (!response.IsSuccessStatusCode) return null!;
        
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Fahrplan>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result is null) return null!;

        return new ConnectionsDto
        {
            Connections = result.Verbindungen.Select(connection => Map(connection, parameters)).ToList(),
            PageEarlier = result.VerbindungReference.Earlier,
            PageLater = result.VerbindungReference.Later,
        };
    }

    public async Task<ConnectionSectionDto?> GetSection(string journeyId)
    {
        var escapedJourneyId = Uri.EscapeDataString(journeyId);
        var response = await http.GetFromJsonAsync<Fahrt>($"reiseloesung/fahrt?journeyId={escapedJourneyId}&poly=true", new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        throw new NotImplementedException();
    }

    public async Task<ConnectionDto?> GetRoute(IncreaseTransferTimeRequestParameters parameters, string transferIncreaseType)
    {
        var request = new ReiseAnfrageTeilstrecke
        {
            OriginalCtxRecon = parameters.ContextId,
            Klasse = parameters.Options.Class == "First" ? "KLASSE_1" : "KLASSE_2",
            FixedTeilstrecke = new FixedTeilstrecke
            {
                Begin = new FixedTeilstreckeStation
                {
                    ExtId = parameters.Begin.ExternalId,
                    Zeitpunkt = parameters.Begin.Time.ConvertToBahnTime()
                },
                End = new FixedTeilstreckeStation
                {
                    ExtId = parameters.End.ExternalId,
                    Zeitpunkt = parameters.End.Time.ConvertToBahnTime()
                },
            },
            AnkunftSuche = transferIncreaseType == "Later" ? "ABFAHRT" : "ANKUNFT",
            Produktgattungen = parameters.Route.RouteOptions[0].GetAllowedTransport(),
            Reisende = generatePassengers(parameters.Passengers),
            SchnelleVerbindungen = false,
            SitzplatzOnly = false,
            BikeCarriage = false,
            ReservierungsKontingenteVorhanden = false,
            NurDeutschlandTicketVerbindungen = false,
            DeutschlandTicketVorhanden = false,
            Zwischenhalte = parameters.Route.ToZwischenhalte(),
            MaxUmstiege = parameters.Options.MaxTransfers,
        };

        var response = await http.PostAsJsonAsync("angebote/teilstrecke", request);
        if (!response.IsSuccessStatusCode) return null!;
        
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Teilstrecke>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result is null) return null!;
        return Map(result.Verbindung, null!, true);
    }

    private ConnectionDto Map(Verbindung connection, RequestParameters parameters, bool transferTimeChanged = false)
    {
        return new ConnectionDto
        {
            Id = connection.TripId,
            TransferTimeChanged = transferTimeChanged,
            ContextId = connection.CtxRecon,
            Sections = connection.VerbindungsAbschnitte.Where(section => section.Halte.Count > 0).Select(section => section.ToDto()).ToList(),
            Price = connection.AngebotsPreis.GetPrice(connection.HasTeilpreis),
            Information = connection.GetInformation(),
            Bike = GetGlobalBikeInformation(connection.FahrradmitnahmeMoeglich),
            Demand = connection.AuslastungsMeldungen.ToDto(),
            BahnRequestUrl = GenerateRequestUrl(parameters, connection.CtxRecon)
        };
    }

    private static string GenerateRequestUrl(RequestParameters requestParameters, string id)
    {
        var so = requestParameters.Route.Origin!.Name;
        var zo = requestParameters.Route.Destination!.Name;
        
        var kl = requestParameters.Options.Class == "First" ? 1 : 2;
        var r = generatePassengerForUrl(requestParameters.Passengers);
        var soid = requestParameters.Route.Origin.Id;
        var zoid = requestParameters.Route.Destination.Id;
        var sot = "ST";
        var zot = "ST";
        var soei = requestParameters.Route.Origin.ExtId;
        var zoei = requestParameters.Route.Destination.ExtId;
        var hd = requestParameters.Time.ConvertToBahnTime();
        var hza = requestParameters.TimeType == "Departure" ? "D" : "A";
        var hz = generateViaUrl(requestParameters.Route);
        var ar = "false";
        var s = "false";
        var d = "false";
        var vm = requestParameters.Route.RouteOptions[0].GetAllowedTransportForUri();
        var fm = "false";
        var bp = "false";
        var dlt = "false";
        var dltv = "false";
        var gh = id;
        //This can be any time below or equal 46 minutes. The bahn.de trip request page will throw an error otherwise
        var mud = requestParameters.Options.MinTransferTime.ToString();

        return
            $"https://www.bahn.de/buchung/fahrplan/suche#sts=true&so={so}&zo={zo}&kl={kl}&mud={mud}&r={r}&soid={soid}&zoid={zoid}&sot={sot}&zot={sot}&soei={soei}&zoei={zoei}&hd={hd}&hza={hza}&hz={hz}&ar={ar}&s={s}&d={d}&vm={vm}&fm={fm}&bp={bp}&dlt={dlt}&dltv={dltv}&gh={gh}&cbs=true";
        
    }

    private static string? GetGlobalBikeInformation(string? bikeData)
    {
        if(bikeData is null) return null;

        return bikeData switch
        {
            "NICHT_BEWERTBAR" => "Unknown",
            "MOEGLICH" => "Yes",
            "NICHT_MOEGLICH" => "No",
            _ => "Unknown"
        };
    }

    private static string generatePassengerForUrl(List<PassengerParameters> passengers)
    {
        string allPassengers = "";

        foreach (var passenger in passengers)
        {
            allPassengers += generateUrlType(passenger);
            allPassengers += ":";
            allPassengers += generateDiscountsForUrl(passenger.Discounts);
            allPassengers += ":1,";

            if (passenger.Bikes > 0)
            {
                allPassengers += $"3:16:KLASSENLOS:{passenger.Bikes},";
            }

            if (passenger.Dogs > 0)
            {
                allPassengers += $"14:16:KLASSENLOS:{passenger.Dogs},";
            }
        }

        if (allPassengers[^1] == ',')
        {
            allPassengers = allPassengers[..^1];
        }
        
        return allPassengers;
    }

    private List<Reisender> generatePassengers(List<PassengerParameters> passengers)
    {
        List<Reisender> convertedPassengers = [];
        
        foreach (var passenger in passengers)
        {
            convertedPassengers.Add(new Reisender{
                Alter = [],
                Anzahl = 1,
                Ermaessigungen = generateDiscounts(passenger.Discounts),
                Typ = generateType(passenger),
            });

            if (passenger.Bikes > 0)
            {
                convertedPassengers.Add(new Reisender
                {
                    Alter = [],
                    Anzahl = passenger.Bikes,
                    Ermaessigungen = generateDiscounts([]),
                    Typ = "FAHRRAD"
                });
            }

            if (passenger.Dogs > 0)
            {
                convertedPassengers.Add(new Reisender
                {
                    Alter = [],
                    Anzahl = passenger.Dogs,
                    Ermaessigungen = generateDiscounts([]),
                    Typ = "HUND"
                });
            }
        }
        
        return convertedPassengers;
    }

    private List<Ermaessigung> generateDiscounts(List<DiscountParameters> discounts)
    {
        if (!discounts.Any())
        {
            return
            [
                new Ermaessigung
                {
                    Art = "KEINE_ERMAESSIGUNG",
                    Klasse = "KLASSENLOS",
                }
            ];
        }
        
        return discounts.Select(discount => new Ermaessigung()
        {
            Art = discount.Type.ToUpper(),
            Klasse = discount.Class == "First" ? "KLASSE_1" : "KLASSE_2",
        }).ToList();
    }

    private static string generateDiscountsForUrl(List<DiscountParameters> discounts)
    {
        if(!discounts.Any()) return "16:KLASSENLOS";
        string all = "";

        for (int i = 0; i < discounts.Count; i++)
        {
            var discount = discounts[i];
            var discountType = discount.Type switch
            {
                "BahnCard25" => 17,
                "BahnCard50" => 23,
                "BahnCard100" => 24,
            };
            
            var @class = discount.Class == "First" ? "KLASSE_1" : "KLASSE_2";

            if (i == 1)
            {
                all += "[";
            }

            if (i > 1)
            {
                all += "|";
            }
            all += $"{discountType}:{@class}";
            if (i > 0 && i == discounts.Count - 1)
            {
                all += "]";
            }
        }
        
        return all;
    }

    private string generateType(PassengerParameters passenger)
    {
        if (passenger.Age is null) throw new InvalidOperationException("Not supported yet");

        if (passenger.Age <= 5) return "KLEINKIND";

        if (passenger.Age <= 14) return "FAMILIENKIND";

        if (passenger.Age <= 26) return "JUGENDLICHER";

        if (passenger.Age <= 64) return "ERWACHSENER";

        return "SENIOR";
    }

    private static string generateUrlType(PassengerParameters passenger)
    {
        if (passenger.Age is null) throw new InvalidOperationException("Not supported yet");

        if (passenger.Age <= 5) return "8";

        if (passenger.Age <= 14) return "11";

        if (passenger.Age <= 26) return "9";

        if (passenger.Age <= 64) return "13";

        return "12";
    }

    private static string generateViaUrl(RouteParameters routeParameters)
    {
        if (routeParameters.Via.Count == 0) return "[]";
        var vias = "[";

        for (int i = 0; i < routeParameters.Via.Count; i++)
        {
            var via = routeParameters.Via[i];
            var id = via.Station.Id;
            var name = via.Station.Name;
            var residecnce = via.Residence;
            var transports = routeParameters.RouteOptions[i + 1].GetAllowedTransportForUri();
            vias += $"[%22{id}%22,%22{name}%22,{residecnce},%22{transports}%22]";
            if (i != routeParameters.Via.Count - 1)
            {
                vias += ",";
            }
        }

        vias += "]";
        return vias;
    }
}