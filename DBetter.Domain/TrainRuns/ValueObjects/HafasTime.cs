namespace DBetter.Domain.TrainRuns.ValueObjects;

public record struct HafasTime(ushort DayOffset, TimeOnly Time)
{
    public TimeSpan AsTimeSpan()
    {
        return Time.ToTimeSpan() + TimeSpan.FromDays(DayOffset);
    }
    
    internal static HafasTime Create(string value)
    {
        value = value.PadLeft(4, '0');

        var dayOffset = 0;
        if (value.Length > 4)
        {
            dayOffset = int.Parse(value[..^4]);
        }
        
        var time = TimeOnly.ParseExact(value[^4..], "HHmm");

        return new HafasTime((ushort)dayOffset, time);
    }
}