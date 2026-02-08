using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public class PlannedRoute
{
    public StationId OriginStationId  { get; private init; }
    
    public MeansOfTransport MeansOfTransportFirstSection { get; private init; }
    
    public Stopover? FirstStopover { get; private init; }
    
    public Stopover? SecondStopover { get; private init; }
    
    public StationId DestinationStationId  { get; private init; }
    
    public TransferAmount MaxTransfers { get; private init; }
    
    public TransferTime MinTransferTime { get; private init; }

    private PlannedRoute(){}

    private PlannedRoute(
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

    public static CanFail<PlannedRoute> Create(
        StationId originStationId,
        MeansOfTransport onFirstSection,
        Stopover? firstStopOver,
        Stopover? secondStopOver,
        StationId destinationStationId,
        TransferAmount maxTransfers,
        TransferTime minTransferTime)
    {
        CanFail<PlannedRoute> transferOptionsResult = new ();
        
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
        
        transferOptionsResult.Succeeded(new PlannedRoute(
            originStationId,
            onFirstSection,
            firstStopOver,
            secondStopOver,
            destinationStationId,
            maxTransfers,
            minTransferTime));
        
        return transferOptionsResult;
    }
}