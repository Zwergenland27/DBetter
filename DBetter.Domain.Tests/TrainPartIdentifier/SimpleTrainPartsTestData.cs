using System.Collections;
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;

namespace DBetter.Domain.Tests.TrainPartIdentifier;

public class SimpleTrainPartsTestData: IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[]{new List<PlannedCoachLayoutId>(){PlannedTrainPartIdentifierTests.FirstLayoutId}},
        new object[]{new List<PlannedCoachLayoutId>(){PlannedTrainPartIdentifierTests.SecondLayoutId}},
        new object[]{new List<PlannedCoachLayoutId>(){PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId}},
        new object[]{new List<PlannedCoachLayoutId>(){PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId}},
    };
    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}