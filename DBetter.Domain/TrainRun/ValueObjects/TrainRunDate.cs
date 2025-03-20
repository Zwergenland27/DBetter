using System.Globalization;
using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.TrainRun.ValueObjects;

public record TrainRunDate(DateTime Value)
{
    public static CanFail<TrainRunDate> ExtractFromJourneyId(BahnJourneyId journeyId)
    {
        Dictionary<string, string> data = journeyId.GetData();

        if (data.TryGetValue("DA", out var dateString) && dateString.Length == 6)
        {
            var germanTime = DateTime.ParseExact(dateString, "ddMMyy", CultureInfo.InvariantCulture);
        
            TimeZoneInfo germanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Berlin");
            return new TrainRunDate(TimeZoneInfo.ConvertTimeToUtc(germanTime, germanTimeZone));
        }

        return DomainErrors.TrainRun.TrainRunDate.Invalid(dateString!);
    }
}