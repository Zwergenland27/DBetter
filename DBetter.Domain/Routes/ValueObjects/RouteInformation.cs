namespace DBetter.Domain.Routes.ValueObjects;

public record RouteInformation(TransportProduct Product, bool ReplacementService, LineNumber? LineNumber, ServiceNumber? ServiceNumber)
{
    public RouteInformation UpdateServiceNumber(ServiceNumber number)
    {
        return this with { ServiceNumber = number };
    }
}