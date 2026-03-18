using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Connections.Entities;
using DBetter.Domain.Connections.Snapshots;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Connections;

internal class TransfersFactory(
    List<SegmentSnapshot> segments,
    MeansOfTransport meansOfTransportFirstRouteSegment,
    Stopover? firstStopover,
    Stopover? secondStopover)
{
    private int? _firstStopoverSegmentIndex;
    private int? _secondStopoverSegmentIndex;
    
    internal List<Transfer> Extract()
    {
        _firstStopoverSegmentIndex = CalculateStopoverSegmentIndex(firstStopover);
        _secondStopoverSegmentIndex = CalculateStopoverSegmentIndex(secondStopover);
        
        List<Transfer> transfers = [];

        byte transferIndex = 0;
        foreach (var transferSegment in segments.OfType<TransferSegmentSnapshot>())
        {
            var previousSegments = GetPreviousTransportSegments(transferSegment.SegmentIndex);
            var followingSegments = GetFollowingTransportSegments(transferSegment.SegmentIndex);

            var firstStopId = previousSegments.First().FirstStationId;
            var previousStopId =  previousSegments.Last().LastStationId;
            var previousDeparture = previousSegments.First().PlannedDepartureTime;
            var previousArrival = previousSegments.Last().PlannedArrivalTime;
            
            var followingStopId = followingSegments.First().FirstStationId;
            var lastStopId = followingSegments.Last().LastStationId;
            var followingDeparture = followingSegments.First().PlannedDepartureTime;
            var followingArrival = followingSegments.Last().PlannedArrivalTime;
            
            var transfer = Transfer.Create(
                new TransferIndex(transferIndex),
                CalculateMeansOfTransportForPreviousSubConnection(transferSegment.SegmentIndex),
                firstStopId, previousDeparture,
                previousStopId, previousArrival,
                CalculateMeansOfTransportForFollowingSubConnection(transferSegment.SegmentIndex),
                followingStopId, followingDeparture,
                lastStopId, followingArrival);
            
            transfers.Add(transfer);
            transferIndex++;
        }

        return transfers;
    }
    
    private List<TransportSegmentSnapshot> GetPreviousTransportSegments(int segmentIndex)
    {
        return segments
            .OfType<TransportSegmentSnapshot>()
            .Where(s => s.SegmentIndex < segmentIndex).OrderBy(s => s.SegmentIndex).ToList();
    }
    
    private List<TransportSegmentSnapshot> GetFollowingTransportSegments(int segmentIndex)
    {
        return segments
            .OfType<TransportSegmentSnapshot>()
            .Where(s => s.SegmentIndex > segmentIndex).OrderBy(s => s.SegmentIndex).ToList();
    }

    private MeansOfTransport CalculateMeansOfTransportForPreviousSubConnection(int transferSegmentIndex)
    {
        if (_firstStopoverSegmentIndex is null || transferSegmentIndex <= _firstStopoverSegmentIndex)
        {
            return new MeansOfTransport(meansOfTransportFirstRouteSegment);
        }

        if (_secondStopoverSegmentIndex is null || transferSegmentIndex <= _secondStopoverSegmentIndex)
        {
            return meansOfTransportFirstRouteSegment
                .Combine(firstStopover!.MeansOfTransportNextSection);
        }

        return meansOfTransportFirstRouteSegment
            .Combine(firstStopover!.MeansOfTransportNextSection)
            .Combine(secondStopover!.MeansOfTransportNextSection);
    }
    
    
    private MeansOfTransport CalculateMeansOfTransportForFollowingSubConnection(int transferSegmentIndex)
    {
        if (_firstStopoverSegmentIndex is null)
        {
            return new MeansOfTransport(meansOfTransportFirstRouteSegment);
        }

        if (transferSegmentIndex < _firstStopoverSegmentIndex)
        {
            if (_secondStopoverSegmentIndex is null)
            {
                return meansOfTransportFirstRouteSegment
                    .Combine(firstStopover!.MeansOfTransportNextSection);
            }

            return meansOfTransportFirstRouteSegment
                .Combine(firstStopover!.MeansOfTransportNextSection)
                .Combine(secondStopover!.MeansOfTransportNextSection);
        }

        if (_secondStopoverSegmentIndex is null)
        {
            return new(firstStopover!.MeansOfTransportNextSection);
        }

        if (transferSegmentIndex < _secondStopoverSegmentIndex)
        {
            return firstStopover!.MeansOfTransportNextSection.Combine(secondStopover!.MeansOfTransportNextSection);
        }
        
        return new(secondStopover!.MeansOfTransportNextSection);
    }

    private int? CalculateStopoverSegmentIndex(Stopover? stopover)
    {
        if (stopover is null) return null;
        foreach (var segment in segments.OfType<TransportSegmentSnapshot>())
        {
            if (segment.LastStationId == stopover.StationId) return segment.SegmentIndex + 1;
        }

        return null;
    }
}