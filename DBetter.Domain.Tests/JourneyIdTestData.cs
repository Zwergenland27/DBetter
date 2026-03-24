using System.Collections;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Tests;

public class JourneyIdTestData: IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return Examples.Select(e => new[] { e }).GetEnumerator();
    }
    
    public static List<JourneyIdValues> Examples =
    [
        new("2|#VN#1#ST#1773857730#PI#0#ZI#257949#TA#0#DA#220326#1S#8011102#1T#2118#LS#8000261#LT#10727#PU#80#RT#1#CA#ICE#ZE#699#ZB#ICE 699#PC#0#FR#8011102#FT#2118#TO#8000261#TT#10727#",
            EvaNumber.Create("8011102").Value,
            EvaNumber.Create("8000261").Value,
            new OperatingDay(new DateOnly(2026, 3, 22)),
            new TrainCirculationIdentifier(
                EvaNumber.Create("8011102").Value,
                new TimeOnly(21, 18),
                EvaNumber.Create("8000261").Value,
                new TravelDuration(609))),
        new("2|#VN#1#ST#1773857730#PI#0#ZI#1272269#TA#6#DA#220326#1S#972142#1T#10024#LS#978591#LT#10107#PU#80#RT#1#CA#STR#ZE#3#ZB#STR 3#PC#8#FR#972142#FT#10024#TO#978591#TT#10107#",
            EvaNumber.Create("972142").Value,
            EvaNumber.Create("978591").Value,
            new OperatingDay(new DateOnly(2026, 3, 23)),
            new TrainCirculationIdentifier(
                EvaNumber.Create("972142").Value,
                new TimeOnly(0, 24),
                EvaNumber.Create("978591").Value,
                new TravelDuration(43)))
    ];

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}