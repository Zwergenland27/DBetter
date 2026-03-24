using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;

namespace DBetter.Infrastructure.TrainCirculations;

public class TrainCirculationPersistenceDto: IPersistenceDto<TrainCirculation>
{
    public required Guid Id { get; set; }
    
    public required string Identifier { get; set; }
    
    public required string Identifier_OriginStationEva {get; set;}
    
    public required TimeOnly Identifier_DepartureTime { get; set; }
    
    public required string Identifier_DestinationStationEva {get; set;}
    
    public required ushort Identifier_DurationMinutes { get; set; }
    public required int ServiceInformation_TransportCategory { get; set; }
    
    public required string? ServiceInformation_LineNumber_Number { get; set; }
    
    public required string? ServiceInformation_LineNumber_ProductClass { get; set; }
    
    public required int? ServiceInformation_ServiceNumber { get; set; }

    public static TrainCirculationPersistenceDto Create(TrainCirculation domain)
    {
        return new TrainCirculationPersistenceDto
        {
            Id = domain.Id.Value,
            Identifier = domain.Identifier.DatabaseFriendly(),
            Identifier_OriginStationEva = domain.Identifier.Origin.Value,
            Identifier_DepartureTime = domain.Identifier.DepartureTime,
            Identifier_DestinationStationEva = domain.Identifier.Destination.Value,
            Identifier_DurationMinutes = domain.Identifier.Duration.Minutes,
            ServiceInformation_TransportCategory = (int)domain.ServiceInformation.TransportCategory,
            ServiceInformation_LineNumber_Number = domain.ServiceInformation.LineNumber?.Number,
            ServiceInformation_LineNumber_ProductClass = domain.ServiceInformation.LineNumber?.ProductClass,
            ServiceInformation_ServiceNumber = domain.ServiceInformation.ServiceNumber?.Value,
        };
    }
    
    public TrainCirculation ToDomain()
    {
        var transportCategory = (TransportCategory) ServiceInformation_TransportCategory;
        
        LineNumber? lineNumber = null;
        if (ServiceInformation_LineNumber_Number is not null)
        {
            lineNumber = new LineNumber(ServiceInformation_LineNumber_Number,
                ServiceInformation_LineNumber_ProductClass);
        }
        
        ServiceNumber? serviceNumber = null;
        if (ServiceInformation_ServiceNumber is not null)
        {
            serviceNumber = new ServiceNumber(ServiceInformation_ServiceNumber.Value);
        }
        
        var serviceInformation = new ServiceInformation(
            transportCategory,
            lineNumber,
            serviceNumber);
        
        return new TrainCirculation(
            new TrainCirculationId(Id),
            new TrainCirculationIdentifier(
                EvaNumber.Create(Identifier_OriginStationEva).Value,
                Identifier_DepartureTime,
                EvaNumber.Create(Identifier_DestinationStationEva).Value,
                new TravelDuration(Identifier_DurationMinutes)),
            serviceInformation);
    }

    public void Apply(TrainCirculation domain)
    {
        Identifier = domain.Identifier.DatabaseFriendly();
        Identifier_OriginStationEva = domain.Identifier.Origin.Value;
        Identifier_DepartureTime = domain.Identifier.DepartureTime;
        Identifier_DestinationStationEva = domain.Identifier.Destination.Value;
        Identifier_DurationMinutes = domain.Identifier.Duration.Minutes;
        ServiceInformation_TransportCategory = (int)domain.ServiceInformation.TransportCategory;
        ServiceInformation_LineNumber_Number = domain.ServiceInformation.LineNumber?.Number;
        ServiceInformation_LineNumber_ProductClass = domain.ServiceInformation.LineNumber?.ProductClass;
        ServiceInformation_ServiceNumber = domain.ServiceInformation.ServiceNumber?.Value;
    }
}

