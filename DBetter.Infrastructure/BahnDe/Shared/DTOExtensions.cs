using DBetter.Application;
using DBetter.Contracts.Shared.DTOs;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;

namespace DBetter.Infrastructure.BahnDe.Shared;

public static class DTOExtensions
{
    private static readonly List<string> _ignoredProductClasses =
    [
        "Bus",
        "STR",
        "Fähre"
    ];
    
    
    public static DateTime? ConvertToDateTime(this string? bahnDateString)
    {
        if (bahnDateString is null) return null;
        var germanTime = DateTime.Parse(bahnDateString);
        
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        if (germanTimeZone.IsInvalidTime(germanTime))
        {
            germanTime = germanTime.AddHours(1);
        }
        return TimeZoneInfo.ConvertTimeToUtc(germanTime, germanTimeZone);
    }
    
    public static Demand GetDemand(this IHasDemandInformation auslastung)
    {
        var firstClassDemand = auslastung.Auslastungsmeldungen
            .Where(m => m.Klasse == Klasse.GetAliasFromComfortClass(ComfortClass.First))
            .Select(a => a.Stufe)
            .FirstOrDefault();
        
        var secondClassDemand = auslastung.Auslastungsmeldungen
            .Where(m => m.Klasse == Klasse.GetAliasFromComfortClass(ComfortClass.Second))
            .Select(a => a.Stufe)
            .FirstOrDefault();

        return new Demand(
            firstClassDemand.ToDomainDemandStatus(),
            secondClassDemand.ToDomainDemandStatus());
    }

    public static TravelTime? GetDepartureTime(this ITrainRunStop stop)
    {
        if(stop.AbfahrtsZeitpunkt is null) return null;

        return new TravelTime(
            stop.AbfahrtsZeitpunkt.ConvertToDateTime()!.Value,
            stop.EzAbfahrtsZeitpunkt.ConvertToDateTime()
        );
    }

    public static TravelTime GetDepartureTime(this VerbindungsAbschnitt verbindungsabschnitt)
    {
        return new TravelTime(
            verbindungsabschnitt.AbfahrtsZeitpunkt.ConvertToDateTime()!.Value,
            verbindungsabschnitt.EzAbfahrtsZeitpunkt.ConvertToDateTime());
    }

    public static TravelTime? GetArrivalTime(this ITrainRunStop stop)
    {
        if(stop.AnkunftsZeitpunkt is null) return null;

        return new TravelTime(
            stop.AnkunftsZeitpunkt.ConvertToDateTime()!.Value,
            stop.EzAnkunftsZeitpunkt.ConvertToDateTime()
        );
    }
    
    public static TravelTime GetArrivalTime(this VerbindungsAbschnitt verbindungsabschnitt)
    {
        return new TravelTime(
            verbindungsabschnitt.AnkunftsZeitpunkt.ConvertToDateTime()!.Value,
            verbindungsabschnitt.EzAnkunftsZeitpunkt.ConvertToDateTime());
    }

    public static bool IsAdditional(this IHasMessage stop)
    {
        return stop.RisNotizen.Any(r => r.Key is "text.realtime.stop.additional");
    }

    public static bool IsCancelled(this IHasMessage stop)
    {
        return stop.RisNotizen.Any(r => r.Key is "text.realtime.stop.cancelled");
    }

    public static bool IsEntryOnly(this IHasMessage stop)
    {
        return stop.RisNotizen.Any(r => r.Key is "text.realtime.stop.exit.disabled");
    }

    public static bool IsExitOnly(this IHasMessage stop)
    {
        return stop.RisNotizen.Any(r => r.Key is "text.realtime.stop.entry.disabled");
    }

    public static StationInfoId? GetStationInfoId(this ITrainRunStop stop){
        if(stop.BahnhofsInfoId is null) return null;

        var stationInfoResult = StationInfoId.Create(stop.BahnhofsInfoId);
        if(stationInfoResult.HasFailed) return null;
        return stationInfoResult.Value;
    }

    public static StationName GetStationName(this ITrainRunStop stop){
        return StationName.Create(stop.Name).Value;
    }

    public static Platform? GetPlatform(this ITrainRunStop stop){
        if(stop.Gleis is null && stop.EzGleis is null) return null;

        var platformType = stop.HaltTyp switch {
            HaltTyp.PL => PlatformType.Platform,
            HaltTyp.ST => PlatformType.BusPlatform,
            _ => PlatformType.Unknown
        };

        if (stop.Gleis is null)
        {
            return new Platform(stop.EzGleis!, null, platformType);
        }

        return new Platform(stop.Gleis, stop.EzGleis, platformType);
    }

    public static StopIndex GetStopIndex(this ITrainRunStop stop){
        return new StopIndex(stop.RouteIdx);
    }

    public static Currency ToCurrency(this Waehrung currency)
    {
        return currency switch
        {
            Waehrung.EUR => Currency.Euro,
            _ => Currency.Unknown
        };
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
            _ => DemandStatus.Unknown
        };
    }
}