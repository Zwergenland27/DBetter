using DBetter.Domain.Connections.ValueObjects;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.Journey.ValueObjects;

public record JourneyInformation
{
    public MeansOfTransport MeansOfTransport { get; private init; }
    public TrainName TrainName { get; private init; }
    public CateringInformation Catering { get; private init; }
    public BikeInformation BikeTransport { get; private init; }

    private JourneyInformation(){}
    
    public JourneyInformation(
        MeansOfTransport meansOfTransport,
        TrainName trainName,
        CateringInformation catering,
        BikeInformation bikeTransport)
    {
        MeansOfTransport = meansOfTransport;
        TrainName = trainName;
        Catering = catering;
        BikeTransport = bikeTransport;
    }
}