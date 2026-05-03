using System.Collections;
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Tests.TrainPartIdentifier;

public class AmbigousMiddleStationTestData: IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[] //First != Last -> Resulting in "middle" index 2
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            PlannedTrainPartIdentifierTests.Route[2].StationId,
            PlannedTrainPartIdentifierTests.Route[3].StationId
        },
        new object[] //First != Last && 2 == Last -> Resulting in "middle" index 1
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            PlannedTrainPartIdentifierTests.Route[1].StationId,
            PlannedTrainPartIdentifierTests.Route[2].StationId
        },
        new object[] //First != Last && 2 == First -> Resulting in "middle" index 3
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            PlannedTrainPartIdentifierTests.Route[3].StationId,
            PlannedTrainPartIdentifierTests.Route[4].StationId
        }
    };
    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}