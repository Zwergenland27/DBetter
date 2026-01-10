using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public class Route
{
    public StationId OriginStationId  { get; private init; }
    
    public MeansOfTransport MeansOfTransportFirstSection { get; private init; }
    
    public Stopover? FirstStopover { get; private init; }
    
    public Stopover? SecondStopover { get; private init; }
    
    public StationId DestinationStationId  { get; private init; }
    
    public TransferAmount MaxTransfers { get; private init; }
    
    public TransferTime MinTransferTime { get; private init; }
    
    public IReadOnlyList<StationId> RequestedStationIds => GetRequestedStationIds();

    private Route(){}

    private Route(
        StationId originStationId,
        MeansOfTransport meansOfTransportFirstSection,
        Stopover? firstStopover,
        Stopover? secondStopover,
        StationId destinationStationId,
        TransferAmount maxTransfers,
        TransferTime minTransferTime)
    {
        OriginStationId = originStationId;
        MeansOfTransportFirstSection = meansOfTransportFirstSection;
        FirstStopover = firstStopover;
        SecondStopover = secondStopover;
        DestinationStationId = destinationStationId;
        MaxTransfers = maxTransfers;
        MinTransferTime = minTransferTime;
    }

    public static CanFail<Route> Create(
        StationId originStationId,
        MeansOfTransport onFirstSection,
        Stopover? firstStopOver,
        Stopover? secondStopOver,
        StationId destinationStationId,
        TransferAmount maxTransfers,
        TransferTime minTransferTime)
    {
        CanFail<Route> transferOptionsResult = new ();
        
        if (firstStopOver is null && secondStopOver is not null)
        {
            transferOptionsResult.Failed(DomainErrors.ConnectionRequest.Route.FirstStopoverMissing);
        }
        
        if (!onFirstSection.AnySelected ||
            (firstStopOver is not null && !firstStopOver.MeansOfTransportNextSection.AnySelected) ||
            (secondStopOver is not null && !secondStopOver.MeansOfTransportNextSection.AnySelected))
        {
            transferOptionsResult.Failed(DomainErrors.ConnectionRequest.Route.NoVehicleAllowed);
        }
        
        transferOptionsResult.Succeeded(new Route(
            originStationId,
            onFirstSection,
            firstStopOver,
            secondStopOver,
            destinationStationId,
            maxTransfers,
            minTransferTime));
        
        return transferOptionsResult;
    }
    
    private List<StationId> GetRequestedStationIds()
    {
        var stationIds = new List<StationId>
        {
            OriginStationId,
            DestinationStationId
        };

        if (FirstStopover is not null)
        {
            stationIds.Add(FirstStopover.StationId);
        }

        if (SecondStopover is not null)
        {
            stationIds.Add(SecondStopover.StationId);
        }

        return stationIds.Distinct().ToList();
    }
}