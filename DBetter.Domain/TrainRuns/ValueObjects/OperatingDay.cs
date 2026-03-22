namespace DBetter.Domain.TrainRuns.ValueObjects;

public record OperatingDay(DateOnly Date)
{
    public static OperatingDay Parse(string value)
    {
        return new(DateOnly.ParseExact(value.PadLeft(6, '0'), "ddMMyy"));
    }

    internal OperatingDay CorrectTimeOffset(HafasTime departureTime)
    {
        var correctedDate = Date.AddDays(departureTime.DayOffset);
        return new OperatingDay(correctedDate);
    }
}