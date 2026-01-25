namespace DBetter.Domain.TrainRuns.ValueObjects;

public record OperatingDay(DateOnly Date)
{
    public static OperatingDay Parse(string value)
    {
        return new(DateOnly.ParseExact(value, "ddMMyy"));
    }
}