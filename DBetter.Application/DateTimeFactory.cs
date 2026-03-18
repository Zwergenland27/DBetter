using System.Globalization;

namespace DBetter.Application;

public static class DateTimeFactory
{
    public static DateTime CreateFromIso8601(string iso8601)
    { 
        return DateTime.Parse(iso8601, null, DateTimeStyles.RoundtripKind);
    }

    public static string ToIso8601(this DateTime date)
    {
        return date.ToString("O");
    }
    
    public static string ToBahnTime(this DateTime date)
    {
        TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
        return TimeZoneInfo.ConvertTimeFromUtc(date, germanTimeZone).ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
