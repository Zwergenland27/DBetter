using DBetter.Domain.Shared;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Shared;

public static class DTOExtensions
{
    public static List<PassengerInfo> GetDomainMessages(this IHasMessage obj)
    {
        return [];
    }
    
    public static List<RoutePassengerInfo> GetDomainSectionMessages(this IHasMessage obj)
    {
        return [];
    }
    
    public static DateTime? ConvertToDateTime(this string? bahnDateString)
    {
        if (bahnDateString is null) return null;
        var germanTime = DateTime.Parse(bahnDateString);
        
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        return TimeZoneInfo.ConvertTimeToUtc(germanTime, germanTimeZone);
    }
    
    public static Demand GetDomainDemand(this List<AuslastungsMeldung> meldungen)
    {
        var firstClassDemand = meldungen
            .Where(m => m.Klasse == Klasse.KLASSE_1)
            .Select(a => a.Stufe)
            .FirstOrDefault();
        
        var secondClassDemand = meldungen
            .Where(m => m.Klasse == Klasse.KLASSE_2)
            .Select(a => a.Stufe)
            .FirstOrDefault();

        return new Demand(
            firstClassDemand.ToDomainDemandStatus(),
            secondClassDemand.ToDomainDemandStatus());
    }

    private static DemandStatus ToDomainDemandStatus(this AuslastungsStufe stufe)
    {
        return stufe switch
        {
            AuslastungsStufe.Unknown => DemandStatus.Unknown,
            AuslastungsStufe.Low => DemandStatus.Low,
            AuslastungsStufe.Medium => DemandStatus.Medium,
            AuslastungsStufe.High => DemandStatus.High,
            AuslastungsStufe.Extreme => DemandStatus.Extreme,
            AuslastungsStufe.Overbooked => DemandStatus.Overbooked,
            _ => throw  new BahnDeException("ConnectionService.ToDemandStatus", $"Unknown demand {stufe}")
        };
    }
}