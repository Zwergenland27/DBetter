using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Infrastructure.TrainCirculations;

public class TrainCirculationPersistenceDto
{
    public required Guid Id { get; set; }
    
    public required int TimeTablePeriod { get; set; }
    
    public required int TrainId { get; set; }
    
    public required int ServiceInformation_TransportCategory { get; set; }
    
    public required string? ServiceInformation_LineNumber_Number { get; set; }
    
    public required string? ServiceInformation_LineNumber_ProductClass { get; set; }
    
    public required int? ServiceInformation_ServiceNumber { get; set; }

    public static TrainCirculationPersistenceDto Create(TrainCirculation domain)
    {
        return new TrainCirculationPersistenceDto
        {
            Id = domain.Id.Value,
            TimeTablePeriod = domain.TimeTablePeriod.Year,
            TrainId = domain.TrainId.Value,
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
            new TimeTablePeriod(TimeTablePeriod),
            new TrainId(TrainId),
            serviceInformation);
    }

    public void Apply(TrainCirculation domain)
    {
        TimeTablePeriod = domain.TimeTablePeriod.Year;
        TrainId = domain.TrainId.Value;
        ServiceInformation_TransportCategory = (int)domain.ServiceInformation.TransportCategory;
        ServiceInformation_LineNumber_Number = domain.ServiceInformation.LineNumber?.Number;
        ServiceInformation_LineNumber_ProductClass = domain.ServiceInformation.LineNumber?.ProductClass;
        ServiceInformation_ServiceNumber = domain.ServiceInformation.ServiceNumber?.Value;
    }
}

