using DBetter.Application;
using DBetter.Contracts.Shared.DTOs;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
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
    public static List<PassengerInfo> GetDomainMessages(this IHasMessage obj)
    {
        return [];
    }
    
    public static List<RoutePassengerInformation> GetDomainSectionMessages(this IHasMessage obj)
    {
        return [];
    }
    
    public static TransportCategory AsDomain(this Produktgattung gattung)
    {
        return gattung switch
        {
            Produktgattung.ICE => TransportCategory.HighSpeedTrain,
            Produktgattung.EC_IC => TransportCategory.FastTrain,
            Produktgattung.IR => TransportCategory.FastTrain,
            Produktgattung.REGIONAL => TransportCategory.RegionalTrain,
            Produktgattung.SBAHN => TransportCategory.SuburbanTrain,
            Produktgattung.BUS => TransportCategory.Bus,
            Produktgattung.SCHIFF => TransportCategory.Boat,
            Produktgattung.UBAHN => TransportCategory.UndergroundTrain,
            Produktgattung.TRAM => TransportCategory.Tram,
            Produktgattung.ERSATZVERKEHR => TransportCategory.Replacement,
            Produktgattung.ANRUFPFLICHTIG => TransportCategory.CallService,
            _ => throw new InvalidDataException()
        };
    }
    
    public static DateTime? ConvertToDateTime(this string? bahnDateString)
    {
        if (bahnDateString is null) return null;
        var germanTime = DateTime.Parse(bahnDateString);
        
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        return TimeZoneInfo.ConvertTimeToUtc(germanTime, germanTimeZone);
    }
    
    public static Demand GetDemand(this IHasDemandInformation auslastung)
    {
        var firstClassDemand = auslastung.Auslastungsmeldungen
            .Where(m => m.Klasse == Klasse.KLASSE_1)
            .Select(a => a.Stufe)
            .FirstOrDefault();
        
        var secondClassDemand = auslastung.Auslastungsmeldungen
            .Where(m => m.Klasse == Klasse.KLASSE_2)
            .Select(a => a.Stufe)
            .FirstOrDefault();

        return new Demand(
            firstClassDemand.ToDomainDemandStatus(),
            secondClassDemand.ToDomainDemandStatus());
    }

    public static DemandDto ToDto(this Demand demand)
    {
        return new DemandDto
        {
            FirstClass = demand.FirstClass.ToString(),
            SecondClass = demand.SecondClass.ToString(),
        };
    }

    public static TravelTime? GetDepartureTime(this IRouteStop stop)
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

    public static TravelTime? GetArrivalTime(this IRouteStop stop)
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

    public static TravelTimeDto? ToDto(this TravelTime? travelTime){
        if(travelTime is null) return null;

        return new TravelTimeDto{
            Planned = travelTime.Planned.ToIso8601(),
            Real = travelTime.Real?.ToIso8601()
        };
    }

    public static StationInfoId? GetStationInfoId(this IRouteStop stop){
        if(stop.BahnhofsInfoId is null) return null;

        var stationInfoResult = StationInfoId.Create(stop.BahnhofsInfoId);
        if(stationInfoResult.HasFailed) return null;
        return stationInfoResult.Value;
    }

    public static StationName GetStationName(this IRouteStop stop){
        return StationName.Create(stop.Name).Value;
    }

    public static Platform? GetPlatform(this IRouteStop stop){
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

    public static PlatformDto? ToDto(this Platform? platform){
        if(platform is null) return null;

        return new PlatformDto {
            Planned = platform.Planned,
            Real = platform.Real,
            Type = platform.Type.ToString()
        };
    }

    public static StopIndex GetStopIndex(this IRouteStop stop){
        return new StopIndex(stop.RouteIdx);
    }

    public static string? GetLine(this ServiceInformation information)
    {
        if (information.LineNumber is null && information.ServiceNumber is null) return null;
        
        var line = information.LineNumber is not null ? information.LineNumber.Value : information.ServiceNumber!.Value.ToString();

        if (!_ignoredProductClasses.Contains(information.ProductClass))
        {
            line = $"{information.ProductClass} {line}";
        }

        return line;
    }
    public static ComfortClass ToComfortClass(this Klasse klasse)
    {
        return klasse switch
        {
            Klasse.KLASSE_1 => ComfortClass.First,
            Klasse.KLASSE_2 => ComfortClass.Second,
            _ => ComfortClass.Unknown
        };
    }

    public static Currency ToCurrency(this Waehrung currency)
    {
        return currency switch
        {
            Waehrung.EUR => Currency.Euro,
            _ => Currency.Unknown
        };
    }

    public static BikeCarriageInformationDto ToDto(this BikeCarriageInformation bikeCarriage){
        return new BikeCarriageInformationDto{
            Status = bikeCarriage.CarriageStatus.ToString(),
            FromStopIndex = bikeCarriage.FromStopIndex.Value,
            ToStopIndex = bikeCarriage.ToStopIndex.Value
        };
    }

    public static CateringInformationDto ToDto(this CateringInformation catering){
        return new CateringInformationDto{
            Type = catering.Type.ToString(),
            FromStopIndex = catering.FromStopIndex.Value,
            ToStopIndex = catering.ToStopIndex.Value
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