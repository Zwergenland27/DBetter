using DBetter.Contracts.Shared.DTOs;
using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Routes.ValueObjects;
using DBetter.Domain.Shared;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Infrastructure.BahnDe.Connections;
using DBetter.Infrastructure.BahnDe.Connections.DTOs;
using DBetter.Infrastructure.BahnDe.Connections.Parameters;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace DBetter.Infrastructure.BahnDe.Shared;

public static class DTOExtensions
{
    public static List<PassengerInfo> GetDomainMessages(this IHasMessage obj)
    {
        return [];
    }
    
    public static List<RoutePassengerInformation> GetDomainSectionMessages(this IHasMessage obj)
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

    public static TravelTime? GetDepartureTime(this IRouteStop stop){
        if(stop.AbfahrtsZeitpunkt is null) return null;

        return new TravelTime(
            stop.AbfahrtsZeitpunkt.ConvertToDateTime()!.Value,
            stop.EzAbfahrtsZeitpunkt.ConvertToDateTime()
        );
    }

    public static TravelTime? GetArrivalTime(this IRouteStop stop){
        if(stop.AnkunftsZeitpunkt is null) return null;

        return new TravelTime(
            stop.AnkunftsZeitpunkt.ConvertToDateTime()!.Value,
            stop.EzAnkunftsZeitpunkt.ConvertToDateTime()
        );
    }

    public static TravelTimeDto? ToDto(this TravelTime? travelTime){
        if(travelTime is null) return null;

        return new TravelTimeDto{
            Planned = travelTime.Planned,
            Real = travelTime.Real
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
        if(stop.Gleis is null) return null;

        var platformType = stop.HaltTyp switch {
            HaltTyp.PL => PlatformType.Platform,
            HaltTyp.ST => PlatformType.BusPlatform,
            _ => PlatformType.Unknown
        };

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

    public static string GetBookingRelevantNumber(this RouteInformation information){
        if(RouteInformationFactory.ServiceNumberIsLineNumber(information.Product)){
            return information.ServiceNumber!.Value.ToString();
        }

        return information.LineNumber!.Value;
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