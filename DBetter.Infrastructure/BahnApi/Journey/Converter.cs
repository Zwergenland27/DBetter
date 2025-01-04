using DBetter.Contracts.Journeys.DTOs;
using DBetter.Contracts.Journeys.Parameters;
using DBetter.Infrastructure.BahnApi.Journey.Parameters;
using DBetter.Infrastructure.BahnApi.Journey.Responses;

namespace DBetter.Infrastructure.BahnApi.Journey;

public static class Converter
{
    private static Dictionary<string, InformationDto?> _risMapping = new()
    {
        {"FT", new InformationDto{Priority = 1, Code = "Ris.DelayReason"}},
        {"QF", new InformationDto{Priority = 2, Code = "Ris.Connection.BusReplacement"}},
        {"DR", new InformationDto{Priority = 1, Code = "Ris.Connection.ControlCenterUnderManned"}},
        {"text.realtime.connection.platform.change", new InformationDto{Priority = 1, Code = "Ris.Platform.Change"}},
        {"text.realtime.connection.cancelled", new InformationDto{Priority = 2, Code = "Ris.Connection.Cancelled"}},
        {"text.realtime.connection.brokentrip", new InformationDto{Priority = 2, Code = "Ris.Connection.GoingToMiss"}},
        {"text.realtime.journey.missed.connection", new InformationDto{Priority = 2, Code = "Ris.Connection.Missed"}},
        {"text.realtime.stop.entry.disabled", new InformationDto{Priority = 0, Code = "Ris.Stop.ExitOnly"}},
        {"text.realtime.stop.exit.disabled", new InformationDto{Priority = 0, Code = "Ris.Stop.EntryOnly"}},
        {"text.realtime.stop.cancelled", new InformationDto{Priority = 2, Code = "Ris.Stop.Cancelled"}},
        {"text.realtime.stop.additional", new InformationDto{Priority = 1, Code = "Ris.Stop.Additional"}},
        
    };
    public static InformationDto ToDto(this RisNotiz info)
    {
        //TODO: Map all FT messages (Always the reason behind the train delay)
        //TODO: Check PriorisierteMeldungen, sometimes it contains information that appear nowhere else and are important
        //Example: Zug aktuell nicht reservierbar
        //TODO: Map all QF messages (always information about vehicle change)
        //Example: Schienenersatzverkehr, Es verkehrt nur NJ40490
        if (_risMapping.TryGetValue(info.Key, out InformationDto? foundInfo))
        {
            return foundInfo!;
        }

        return new InformationDto
        {
            Code = info.Key,
            Priority = 2
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