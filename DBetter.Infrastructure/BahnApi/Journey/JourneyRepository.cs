using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using DBetter.Contracts.Journeys.DTOs;
using DBetter.Contracts.Journeys.Parameters;
using DBetter.Domain.Users.ValueObjects;
using DBetter.Infrastructure.BahnApi.Journey.Parameters;
using DBetter.Infrastructure.BahnApi.Journey.Responses;
using Microsoft.AspNetCore.Builder;

namespace DBetter.Infrastructure.BahnApi.Journey;

public class JourneyRepository(HttpClient http)
{
    public async Task<List<ConnectionDto>> GetRoutes(RequestParameters parameters)
    {
        TimeZoneInfo germanTimeZone = TimeZoneInfo.Local;
        DateTime germanRequestTime = TimeZoneInfo.ConvertTimeFromUtc(parameters.Time!, germanTimeZone);
        var request = new ReiseAnfrage
        {
            AbfahrtsHalt = parameters.Route.Origin!.Id,
            AnkunftsHalt = parameters.Route.Destination!.Id,
            AnfrageZeitpunkt = germanRequestTime.ToString("yyyy-MM-ddTHH:mm:ss"),
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
            MaxUmstiege = parameters.Options.MaxTransfers
        };

        var response = await http.PostAsJsonAsync("angebote/fahrplan", request);
        if (!response.IsSuccessStatusCode) return [];
        
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Fahrplan>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result is null) return [];
        
        return result.Verbindungen.Select(connection => Map(connection)).ToList();
    }

    private ConnectionDto Map(Verbindung connection)
    {
        return new ConnectionDto
        {
            Id = connection.TripId,
            Sections = connection.VerbindungsAbschnitte.Where(section => section.Halte.Count > 0).Select(section => new ConnectionSectionDto
            {
                LineNr = section.Verkehrsmittel!.MittelText,
                Catering = GetCateringInformation(section),
                Bike = GetBikeInformation(section),
                Accessibility = GetAccessibilityInformation(section),
                Demand = section.AuslastungsMeldungen.ToDto(),
                Information = CollectInformation(section.RisNotizen, section.HimMeldungen,
                    section.PriorisierteMeldungen),
                Vehicle = null,
                Percentage = section.AbschnittsAnteil,
                Stops = section.Halte.Select(stop => new ConnectionStationDto
                {
                    Id = stop.Id,
                    Name = stop.Name,
                    RouteIndex = stop.RouteIdx,
                    Arrival = ConvertToDateTime(stop.AnkunftsZeitpunkt),
                    RealTimeArrival = ConvertToDateTime(stop.EzAnkunftsZeitpunkt),
                    Departure = ConvertToDateTime(stop.AbfahrtsZeitpunkt),
                    RealTimeDeparture = ConvertToDateTime(stop.EzAnkunftsZeitpunkt),
                    Information = CollectInformation(stop.RisNotizen, stop.HimMeldungen, stop.PriorisierteMeldungen),
                    Demand = stop.AuslastungsMeldungen.ToDto(),

                }).ToList()
            }).ToList(),
            Price = connection.AngebotsPreis.GetPrice(connection.HasTeilpreis),
            Information = CollectInformation(connection.RisNotizen, connection.HimMeldungen,
                connection.PriorisierteMeldungen),
            Bike = GetGlobalBikeInformation(connection.FahrradmitnahmeMoeglich),
            Demand = connection.AuslastungsMeldungen.ToDto(),
        };
    }

    private static string GetCateringInformation(Verbindungsabschnitt section)
    {
        var vehicle = section.Verkehrsmittel!;

        if (vehicle.ProduktGattung is not "ICE" and not "EC_IC" and not "IR") return "Unknown";
        
        if (vehicle.ZugAttribute.Any(a => a.Key == "BR"))
        {
            return CheckPartial("Restaurant", section, vehicle.ZugAttribute.First(a => a.Key == "BR"));
        }

        if (vehicle.ZugAttribute.Any(a => a.Key == "MP"))
        {
            return "SnackService";
        }

        if (vehicle.ZugAttribute.Any(a => a.Key == "SN"))
        {
            return CheckPartial("Snack", section, vehicle.ZugAttribute.First(a => a.Key == "SN"));
        }

        return "None";
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

    private static string GetBikeInformation(Verbindungsabschnitt section)
    {
        var vehicle = section.Verkehrsmittel!;

        if (section.HimMeldungen.Any(info => info.Text is not null && info.Text.Contains("Die Mitnahme von Fahrrädern ist nicht möglich.")))
        {
            return "No";
        }

        if (vehicle.ZugAttribute.Any(a => a.Key == "FR"))
        {
            return "ReservationRequired";
        }
        
        if (vehicle.ZugAttribute.Any(a => a.Key == "FB"))
        {
            return "Limited";
        }
        
        return "Unknown";
    }

    private static string CheckPartial(string name, Verbindungsabschnitt section, ZugAttribut attribute)
    {
        var startStation = section.Halte[0].Name;
        var endStation = section.Halte[^1].Name;
        
        var sectionPart = attribute.TeilstreckenHinweis;

        if (sectionPart is null) return name;
        
        var bracesRemoved = sectionPart.Substring(1, sectionPart.Length - 2);
        var stations = bracesRemoved.Split(" - ");

        if (stations[0] == startStation && stations[1] == endStation)
        {
            return name;   
        }

        return $"Partial{name}";
    }

    private static string GetAccessibilityInformation(Verbindungsabschnitt section)
    {
        var vehicle = section.Verkehrsmittel!;
        if (vehicle.ZugAttribute.Any(a => a.Key is "RZ"))
        {
            return CheckPartial("Accessible", section, vehicle.ZugAttribute.First(a => a.Key == "RZ"));
        }
        
        if (vehicle.ZugAttribute.Any(a => a.Key is "EH"))
        {
            return CheckPartial("Accessible", section, vehicle.ZugAttribute.First(a => a.Key == "EH"));
        }
        return "Unknown";
    }

    private static DateTime? ConvertToDateTime(string? dateString)
    {
        CultureInfo germanCulture = CultureInfo.CurrentCulture;
        if (dateString == null)
        {
            return null;
        }

        return DateTime.ParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", germanCulture);
    }

    private List<InformationDto> CollectInformation(List<RisNotiz> risInfos, List<HimMeldung> himInfos,
        List<PriorisierteMeldung> prioritizedInfos)
    {
        return risInfos.Select(info => info.ToDto()).ToList();
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

    private string generateType(PassengerParameters passenger)
    {
        if (passenger.Age is null) throw new InvalidOperationException("Not supported yet");

        if (passenger.Age <= 5) return "KLEINKIND";

        if (passenger.Age <= 14) return "FAMILIENKIND";

        if (passenger.Age <= 26) return "JUGENDLICHER";

        if (passenger.Age <= 64) return "ERWACHSENER";

        return "SENIOR";
    }
}