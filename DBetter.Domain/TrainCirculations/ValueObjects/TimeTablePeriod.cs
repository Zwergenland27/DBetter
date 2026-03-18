using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record TimeTablePeriod(int Year)
{
    public static TimeTablePeriod FromOperatingDay(OperatingDay operatingDay)
    {
        var timeTableChangeDay = GetTimeTableChangeDay(operatingDay.Date.Year);
        return operatingDay.Date > timeTableChangeDay ?
            new TimeTablePeriod(timeTableChangeDay.Year + 1) : 
            new TimeTablePeriod(timeTableChangeDay.Year);
    }

    private static DateOnly GetTimeTableChangeDay(int year)
    {
        var firstSaturday = new DateOnly(year, 12, 1);
        while (firstSaturday.DayOfWeek is not DayOfWeek.Saturday)
        {
            firstSaturday = firstSaturday.AddDays(1);
        }

        return firstSaturday.AddDays(7);
    } 
}