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

    public static Dictionary<string, int> _priorityMapping = new()
    {
        { "FT", 1 },
        { "QF", 2 },
        { "DR", 1 },
    };
    
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