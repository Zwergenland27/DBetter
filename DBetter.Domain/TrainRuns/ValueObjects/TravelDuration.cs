namespace DBetter.Domain.TrainRuns.ValueObjects;

public record struct TravelDuration(ushort Minutes)
{
    internal static TravelDuration Create(HafasTime departureTime, HafasTime arrivalTime)
    {
        var departure = departureTime.AsTimeSpan();
        var arrival = arrivalTime.AsTimeSpan();
        var totalMinutes = (int)(arrival - departure).TotalMinutes;
        return new TravelDuration((ushort)totalMinutes);
    }
}