using System.Collections;
using DBetter.Domain.PlannedCoachLayouts.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCompositions.TrainParts;

namespace DBetter.Domain.Tests.TrainPartIdentifier;

public class ResolvedAmbigousMiddleStationTestData: IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[] //Layout 1: Route[0] - Route[2], Layout 2: Route[2] - Route[End]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[1].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[2].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[2].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.SecondLayoutId),
            }
        },
        new object[] //Layout 1: Route[0] - Route[1], Layout 2: Route[0] - Route[End]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[1].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[2].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.SecondLayoutId),
            }
        },
        new object[] //Layout 1: Route[0] - Route[1], Layout 2: Route[0] - Route[End]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[1].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[2].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.SecondLayoutId),
            }
        },
        new object[] //Layout 1: Route[0] - Route[End], Layout 2: Route[3] - Route[End]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[3].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[3].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.SecondLayoutId),
            }
        },
        new object[] //Layout 1 + Layout 2: Route[0] - Route[End], Layout 3: Route[3] - Route[End]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[3].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId, PlannedTrainPartIdentifierTests.ThirdLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId, PlannedTrainPartIdentifierTests.ThirdLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.SecondLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[3].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.ThirdLayoutId),
            }
        },
        new object[] //Layout 1 + Layout 2: Route[0] - Route[End], Layout 3: Route[3] - Route[End]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[3].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.ThirdLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.ThirdLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[3].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.ThirdLayoutId),
            }
        },
        new object[] //Layout 1: Route[0] - Route[End], Layout 2: Route[3] - Route[End] Layout 3: Route[2] - Route[3]
        {
            new List<(StationId, List<PlannedCoachLayoutId>)>
            {
                (PlannedTrainPartIdentifierTests.Route[0].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[1].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.ThirdLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[3].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId, PlannedTrainPartIdentifierTests.ThirdLayoutId]),
                (PlannedTrainPartIdentifierTests.Route[^2].StationId, [PlannedTrainPartIdentifierTests.FirstLayoutId, PlannedTrainPartIdentifierTests.SecondLayoutId]),
            },
            new List<PlannedTrainPart>
            {
                new (PlannedTrainPartIdentifierTests.Route[0].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.FirstLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[3].StationId, PlannedTrainPartIdentifierTests.Route[^1].StationId, PlannedTrainPartIdentifierTests.SecondLayoutId),
                new (PlannedTrainPartIdentifierTests.Route[2].StationId, PlannedTrainPartIdentifierTests.Route[4].StationId, PlannedTrainPartIdentifierTests.ThirdLayoutId),
            }
        }
    };
    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}