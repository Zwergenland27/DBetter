namespace DBetter.Domain.TrainCirculations.ValueObjects;

public record ServiceInformation
{
    public TransportCategory TransportCategory { get; private init; }
    
    public LineNumber? LineNumber { get; private init; }
    
    public ServiceNumber? ServiceNumber { get; private init; }

    public ServiceInformation(TransportCategory transportCategory, LineNumber? lineNumber, ServiceNumber? serviceNumber)
    {
        TransportCategory = transportCategory;
        LineNumber = lineNumber;
        ServiceNumber = serviceNumber;
    }
    
    private ServiceInformation()
    {
    }

    internal ServiceInformation UpdateServiceNumber(ServiceNumber number)
    {
        return this with { ServiceNumber = number };
    }

    internal ServiceInformation UpdateLineNumber(LineNumber newLineNumber)
    {
        if (TransportCategory is not (TransportCategory.HighSpeedTrain or TransportCategory.FastTrain)) return this;
        
        if (LineNumber is null)
        {
            return this with { LineNumber = newLineNumber };
        }

        return this with { LineNumber = LineNumber.Update(newLineNumber) };
    }
}