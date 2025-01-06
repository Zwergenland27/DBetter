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
using Microsoft.AspNetCore.Routing;

namespace DBetter.Infrastructure.BahnApi.Journey;

public class JourneyRepository(HttpClient http)
{
    public async Task<ConnectionsDto?> GetRoutes(RequestParameters parameters, string? page)
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
            Connections = result.Verbindungen.Select(connection => Map(connection)).ToList(),
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

    private ConnectionDto Map(Verbindung connection)
    {
        return new ConnectionDto
        {
            Id = connection.TripId,
            Sections = connection.VerbindungsAbschnitte.Where(section => section.Halte.Count > 0).Select(section => section.ToDto()).ToList(),
            Price = connection.AngebotsPreis.GetPrice(connection.HasTeilpreis),
            Information = connection.GetInformation(),
            Bike = GetGlobalBikeInformation(connection.FahrradmitnahmeMoeglich),
            Demand = connection.AuslastungsMeldungen.ToDto(),
        };
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