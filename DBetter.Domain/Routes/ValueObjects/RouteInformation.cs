namespace DBetter.Domain.Route.ValueObjects;

public record RouteInformation(TransportProduct Product, LineNumber? Line, ServiceNumber? Number)
{
    public RouteInformation UpdateServiceNumber(ServiceNumber number)
    {
        return this with { Number = number };
    }
}