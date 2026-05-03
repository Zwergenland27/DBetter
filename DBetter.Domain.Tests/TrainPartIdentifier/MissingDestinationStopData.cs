using System.Collections;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Tests.TrainPartIdentifier;

public class MissingDestinationStopTestData: IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]{new List<StationId>{PlannedTrainPartIdentifierTests.Route[0].StationId}},
        new object[]{new List<StationId>{PlannedTrainPartIdentifierTests.Route[5].StationId}},
        new object[]{new List<StationId>{PlannedTrainPartIdentifierTests.Route[2].StationId, PlannedTrainPartIdentifierTests.Route[3].StationId}},
    };
    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

}