using System.Collections;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Tests;

public class TravelDurationTestData:  IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return Examples.Select(e => new object[] { e.Item1, e.Item2, e.Item3 }).GetEnumerator();
    }
    
    public static List<(HafasTime, HafasTime, ushort)> Examples =
    [
        new(
            new HafasTime(0, new TimeOnly(0, 0)),
            new HafasTime(0, new TimeOnly(0, 1)),
            1),
        new(
            new HafasTime(1, new TimeOnly(0, 18)),
            new HafasTime(1, new TimeOnly(1, 19)),
            61),
        new(
            new HafasTime(0, new TimeOnly(0, 0)),
            new HafasTime(1, new TimeOnly(0, 1)),
            1441),
        new(
            new HafasTime(0, new TimeOnly(21, 18)),
            new HafasTime(0, new TimeOnly(21, 28)),
            10),
        new(
            new HafasTime(0, new TimeOnly(21, 18)),
            new HafasTime(1, new TimeOnly(7, 27)),
            609),
    ];

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}