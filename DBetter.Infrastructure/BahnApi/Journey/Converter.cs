using System.Globalization;
using DBetter.Contracts;
using DBetter.Contracts.Journeys.DTOs;
using DBetter.Contracts.Journeys.Parameters;
using DBetter.Infrastructure.BahnApi.Journey.Parameters;
using DBetter.Infrastructure.BahnApi.Journey.Responses;

namespace DBetter.Infrastructure.BahnApi.Journey;

public static class Converter
{
    private static Dictionary<string, Tuple<int, string>> _codeMapping = new()
    {
        {"text.realtime.connection.platform.change", new Tuple<int, string>(1, "Ris.Platform.Change")},
        {"text.realtime.connection.cancelled", new Tuple<int, string>(2, "Ris.Connection.Cancelled")},
        {"text.realtime.connection.brokentrip", new Tuple<int, string>(2, "Ris.Connection.GoingToMiss")},
        {"text.realtime.journey.missed.connection", new Tuple<int, string>(2, "Ris.Connection.Missed")},
        {"text.realtime.stop.entry.disabled", new Tuple<int, string>(0, "Ris.Stop.ExitOnly")},
        {"text.realtime.stop.exit.disabled", new Tuple<int, string>(0, "Ris.Stop.EntryOnly")},
        {"text.realtime.stop.cancelled", new Tuple<int, string>(2, "Ris.Stop.Cancelled")},
        {"text.realtime.stop.additional", new Tuple<int, string>(1, "Ris.Stop.Additional")},
    };

    public static List<InformationDto> GetInformation(this Verbindung connection)
    {
        return CollectInformation(connection.RisNotizen, connection.HimMeldungen, connection.PriorisierteMeldungen);
    }
    
    public static List<InformationDto> GetInformation(this Verbindungsabschnitt section)
    {
        return CollectInformation(section.RisNotizen, section.HimMeldungen, section.PriorisierteMeldungen);
    }
    
    public static List<InformationDto> GetInformation(this Halt stop)
    {
        return CollectInformation(stop.RisNotizen, stop.HimMeldungen, stop.PriorisierteMeldungen);
    }
    
    private static List<InformationDto> CollectInformation(List<RisNotiz> risInfos, List<HimMeldung> himInfos,
        List<PriorisierteMeldung> prioritizedInfos)
    {
        return risInfos.Select(info => info.ToDto()).ToList();
    }

    public static Dictionary<string, int> _priorityMapping = new()
    {
        { "FT", 1 },
        { "QF", 2 },
        { "DR", 1 },
    };

    public static ConnectionSectionDto ToDto(this Verbindungsabschnitt section)
    {
        return new ConnectionSectionDto
        {
            LineNameShort = section.Verkehrsmittel!.KurzText,
            LineNameMedium = section.Verkehrsmittel!.MittelText,
            LineNameFull = section.Verkehrsmittel!.LangText,
            Direction = section.Verkehrsmittel!.Richtung,
            Catering = section.GetCateringInformation(),
            Bike = section.GetBikeInformation(),
            Accessibility = section.GetAccessibilityInformation(),
            Demand = section.AuslastungsMeldungen.ToDto(),
            Information = section.GetInformation(),
            ConnectionPrediction = section.GetConnectionPrediction(),
            Vehicle = null,
            Percentage = section.AbschnittsAnteil,
            ReservationRequired = section.ReservierungspflichtigNote == "Reservierungspflicht",
            JourneyId = section.JourneyId,
            Stops = section.Halte.Select(stop => stop.ToDto()).ToList()
        };
    }

    public static string GetConnectionPrediction(this Verbindungsabschnitt section)
    {
        return section.AnschlussBewertungCode switch
        {
            2 => "Reachable",
            _ => "Unknown"
        };
    }

    public static ConnectionStationDto ToDto(this Halt stop)
    {
        return new ConnectionStationDto
        {
            Id = stop.Id,
            Name = stop.Name,
            RouteIndex = stop.RouteIdx,
            Arrival = stop.AnkunftsZeitpunkt.ConvertToDateTime(),
            RealTimeArrival = stop.EzAnkunftsZeitpunkt.ConvertToDateTime(),
            Departure = stop.AbfahrtsZeitpunkt.ConvertToDateTime(),
            RealTimeDeparture = stop.EzAbfahrtsZeitpunkt.ConvertToDateTime(),
            Information = stop.GetInformation(),
            Demand = stop.AuslastungsMeldungen.ToDto(),
            Platform = stop.Gleis,
            ExternalId = stop.ExtId
        };
    }
    
    private static string GetCateringInformation(this Verbindungsabschnitt section)
    {
        var vehicle = section.Verkehrsmittel!;

        if (vehicle.ProduktGattung is not "ICE" and not "EC_IC" and not "IR") return "Unknown";
        
        if (vehicle.ZugAttribute.Any(a => a.Key == "BR"))
        {
            return CheckPartial("Restaurant", section, vehicle.ZugAttribute.First(a => a.Key == "BR"));
        }

        if (vehicle.ZugAttribute.Any(a => a.Key == "QP"))
        {
            return "Bistro";
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
    
    private static string GetBikeInformation(this Verbindungsabschnitt section)
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
    
    private static string GetAccessibilityInformation(this Verbindungsabschnitt section)
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
    
    public static DateTime? ConvertToDateTime(this string? dateString)
    {
        //Something is wrong about the time format here!
        CultureInfo germanCulture = CultureInfo.CurrentCulture;
        if (dateString == null)
        {
            return null;
        }
        
        return DateTime.ParseExact(dateString, "yyyy-MM-ddTHH:mm:ss", germanCulture).ToUniversalTime();
    }

    public static string ConvertToBahnTime(this DateTime date)
    {
        TimeZoneInfo germanTimeZone = TimeZoneInfo.Local;
        DateTime germanTime = TimeZoneInfo.ConvertTimeFromUtc(date, germanTimeZone);
        return germanTime.ToString("yyyy-MM-ddTHH:mm:ss");
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
    
    public static InformationDto ToDto(this RisNotiz info)
    {
        if (_codeMapping.TryGetValue(info.Key, out var foundInfo))
        {
            return new InformationDto
            {
                Priority = foundInfo.Item1,
                Code = foundInfo.Item2,
                Text = info.Value
            };
        }

        if (_priorityMapping.TryGetValue(info.Key, out var priority))
        {
            return new InformationDto
            {
                Priority = priority,
                Code = info.Key,
                Text = info.Value
            };
        }

        return new InformationDto
        {
            Priority = 2,
            Code = info.Key,
            Text = info.Value
        };
    }

    public static PriceDto? GetPrice(this PreisAngebot? priceOffer, bool sectionPrice)
    {
        if (priceOffer is null) return null;
        return new PriceDto
        {
            Value = priceOffer.Betrag,
            Currency = priceOffer.Waehrung,
            SectionPrice = sectionPrice
        };
    }
    
    public static DemandDto ToDto(this List<AuslastungsMeldung> demand)
    {
        return new DemandDto
        {
            FirstClass = GetDemandInfos("KLASSE_1", demand),
            SecondClass = GetDemandInfos("KLASSE_2", demand),
        };
    }
    
    public static List<string> GetAllowedTransport(this RouteOptionParameters options)
    {
        List<string> allowedTransport = [];

        if (options.AllowHighSpeedTrains)
        {
            allowedTransport.Add("ICE");
        }

        if (options.AllowIntercityTrains)
        {
            allowedTransport.Add("EC_IC");
            allowedTransport.Add("IR");
        }

        if (options.AllowRegionalTrains)
        {
            allowedTransport.Add("REGIONAL");
        }

        if (options.AllowPublicTransport)
        {
            allowedTransport.Add("SBAHN");
            allowedTransport.Add("BUS");
            allowedTransport.Add("SCHIFF");
            allowedTransport.Add("UBAHN");
            allowedTransport.Add("TRAM");
        }


        return allowedTransport;
    }

    public static List<Zwischenhalt> ToZwischenhalte(this RouteParameters routeParameters)
    {
        List<Zwischenhalt> vias = [];
        for (int i = 0; i < routeParameters.Via.Count; i++)
        {
            int residenceTime = routeParameters.Via[i].Residence;
            vias.Add(new Zwischenhalt
            {
                Aufenthaltsdauer = residenceTime == 0 ? null : residenceTime,
                Id = routeParameters.Via[i].Station.Id,
                VerkehrsmittelOfNextAbschnitt = routeParameters.RouteOptions[i + 1].GetAllowedTransport()
            });
        }
        return vias;
    }

    private static string GetDemandInfos(string @class, List<AuslastungsMeldung> demands)
    {
        var demandObject = demands.FirstOrDefault(d => d.Klasse == @class);
        string demand = "Unknown";
        if (demandObject is not null)
        {
            demand = demandObject.Stufe switch
            {
                0 => "Unknown",
                1 => "Low",
                2 => "Medium",
                3 => "High",
                4 => "Extreme",
                _ => "Unknown"
            };
        }

        return demand;
    }
}