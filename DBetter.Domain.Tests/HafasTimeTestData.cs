using System.Collections;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Domain.Tests;

public class HafasTimeTestData: IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        return Examples.Select(e => new object[] { e.Item1, e.Item2 }).GetEnumerator();
    }
    
    public static List<(string, HafasTime)> Examples =
    [
        new("", new HafasTime(0, new TimeOnly(0, 0))),
        new("0", new HafasTime(0, new TimeOnly(0, 0))),
        new("1", new HafasTime(0, new TimeOnly(0, 1))),
        new("10001", new HafasTime(1, new TimeOnly(0, 1))),
        new("27", new HafasTime(0, new TimeOnly(0, 27))),
        new("10027", new HafasTime(1, new TimeOnly(0, 27))),
        new("127", new HafasTime(0, new TimeOnly(1, 27))),
        new("10127", new HafasTime(1, new TimeOnly(1, 27))),
        new("1227", new HafasTime(0, new TimeOnly(12, 27))),
        new("11227", new HafasTime(1, new TimeOnly(12, 27))),
    ];
    

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}