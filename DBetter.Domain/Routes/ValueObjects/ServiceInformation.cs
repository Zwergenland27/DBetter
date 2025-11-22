namespace DBetter.Domain.Routes.ValueObjects;

public record ServiceInformation(TransportCategory TransportCategory, string ProductClass, LineNumber? LineNumber, ServiceNumber? ServiceNumber)
{
    public ServiceInformation UpdateServiceNumber(ServiceNumber number)
    {
        return this with { ServiceNumber = number };
    }
}