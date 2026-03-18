using DBetter.Application.Abstractions.Caching;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainRuns.Queries.Get;

public class CacheDurationStrategy(TravelTime DepartureTime, TravelTime ArrivalTime)
{
    public CachingOptions GetOptimalCachingOptions()
    {
        var duration = TimeSpan.FromMinutes(20);

        var timeUntilDeparture = DepartureTime.Planned - DateTime.UtcNow;
        var timeUntilArrival =  (ArrivalTime.Real ?? ArrivalTime.Planned) - DateTime.UtcNow;
        if (timeUntilDeparture <= TimeSpan.Zero && timeUntilArrival <= TimeSpan.Zero)
        {
            timeUntilArrival = timeUntilArrival.Negate();
            if (timeUntilArrival <= TimeSpan.FromHours(1))
            {
                duration = TimeSpan.FromSeconds(20);   
            }else if (timeUntilArrival <= TimeSpan.FromHours(6))
            {
                duration = TimeSpan.FromMinutes(5);
            }
            else
            {
                duration = TimeSpan.FromHours(4);
            }
        }
        else if (timeUntilDeparture <= TimeSpan.Zero && timeUntilDeparture > TimeSpan.Zero)
        {
            duration = TimeSpan.FromSeconds(20);
        }else
        {
            if (timeUntilDeparture >= TimeSpan.FromDays(14))
            {
                duration = TimeSpan.FromHours(4);
            }else if (timeUntilDeparture >= TimeSpan.FromDays(7))
            {
                duration = TimeSpan.FromHours(2);
            }else if (timeUntilDeparture >= TimeSpan.FromDays(1))
            {
                duration = TimeSpan.FromMinutes(30);
            }else if (timeUntilDeparture >= TimeSpan.FromHours(16))
            {
                duration = TimeSpan.FromMinutes(5);
            }else if (timeUntilDeparture >= TimeSpan.FromHours(8))
            {
                duration = TimeSpan.FromMinutes(1);
            }
            else if (timeUntilDeparture >= TimeSpan.FromHours(2))
            {
                duration = TimeSpan.FromSeconds(30);
            }
            else
            {
                duration = TimeSpan.FromSeconds(20);
            }
        }

        return new CachingOptions
        {
            Duration = duration
        };
    }
}