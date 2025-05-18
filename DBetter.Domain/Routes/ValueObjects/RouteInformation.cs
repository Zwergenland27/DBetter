namespace DBetter.Domain.Routes.ValueObjects;

public record RouteInformation(string ServiceCategory, bool ReplacementService, LineNumber? LineNumber, ServiceNumber? ServiceNumber)
{
    public RouteInformation UpdateServiceNumber(ServiceNumber number)
    {
        return this with { ServiceNumber = number };
    }
}