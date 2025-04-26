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
    
    public int MaxTransfers { get; private init; }
    
    public int MinTransferTime { get; private init; }

    private Route(){}

    private Route(
        StationId originStationId,
        MeansOfTransport meansOfTransportFirstSection,
        Stopover? firstStopover,
        Stopover? secondStopover,
        StationId destinationStationId,
        int maxTransfers,
        int minTransferTime)
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
        int maxTransfers,
        int minTransferTime)
    {
        CanFail<Route> transferOptionsResult = new CanFail<Route>();
        if (maxTransfers < 0)
        {
            transferOptionsResult.Failed(DomainErrors.ConnectionRequest.Route.MaxTransfers.NegativeNotAllowed);
        }
        else if (maxTransfers > 10)
        {
            transferOptionsResult.Failed(DomainErrors.ConnectionRequest.Route.MaxTransfers.Max10);
        }
        
        if (minTransferTime < 0)
        {
            transferOptionsResult.Failed(DomainErrors.ConnectionRequest.Route.MinTransferTime.NegativeNotAllowed);
        }
        else if (minTransferTime > 43)
        {
            transferOptionsResult.Failed(DomainErrors.ConnectionRequest.Route.MinTransferTime.Max43);
        }
        
        if (firstStopOver is null && secondStopOver is not null)
        {
            return DomainErrors.ConnectionRequest.Route.FirstStopoverMissing;
        }
        
        if (!onFirstSection.AnySelected ||
            (firstStopOver is not null && !firstStopOver.MeansOfTransportNextSection.AnySelected) ||
            (secondStopOver is not null && !secondStopOver.MeansOfTransportNextSection.AnySelected))
        {
            return DomainErrors.ConnectionRequest.Route.NoVehicleAllowed;
        }
        
        return new Route(
            originStationId,
            onFirstSection,
            firstStopOver,
            secondStopOver,
            destinationStationId,
            maxTransfers,
            minTransferTime);
    }
}