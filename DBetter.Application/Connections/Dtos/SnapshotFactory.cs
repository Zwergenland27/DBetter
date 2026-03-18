using DBetter.Application.Requests.Dtos;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Stations;

namespace DBetter.Application.Connections.Dtos;

public class SnapshotFactory(List<SegmentDto> segments, List<Station> stations)
{
    public List<SegmentSnapshot> ToSnapshot()
    {
        var segmentSnapshots = new List<SegmentSnapshot>();
        byte segmentIndex = 0;
        foreach (var segment in segments)
        {
            if (segment is TransferSegmentDto)
            {
                segmentSnapshots.Add(new TransferSegmentSnapshot
                {
                    SegmentIndex = segmentIndex,
                });
            }

            if (segment is TransportSegmentDto transportSegment)
            {
                var firstStop = transportSegment.Stops.First();
                var lastStop = transportSegment.Stops.Last();
                segmentSnapshots.Add(new TransportSegmentSnapshot
                {
                    SegmentIndex = segmentIndex,
                    FirstStationId = stations.First(s => s.EvaNumber == firstStop.EvaNumber).Id,
                    PlannedDepartureTime = firstStop.DepartureTime!.Planned,
                    LastStationId = stations.Last(s => s.EvaNumber == lastStop.EvaNumber).Id,
                    PlannedArrivalTime = lastStop.ArrivalTime!.Planned
                });
            }
            segmentIndex++;
        }

        return segmentSnapshots;
    }
}