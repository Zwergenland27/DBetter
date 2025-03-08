using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;
using DBetter.Domain.Stations.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public class Route
{
    private List<EvaNumber> _stops = [];
    
    private List<AllowedVehicles> _allowedVehicles = [];
    
    public IReadOnlyList<EvaNumber> Stops => _stops.AsReadOnly();
    
    public IReadOnlyList<AllowedVehicles> AllowedVehicles => _allowedVehicles.AsReadOnly();

    private Route(
        List<EvaNumber> stops,
        List<AllowedVehicles> allowedVehicles)
    {
        _stops = stops;
        _allowedVehicles = allowedVehicles;
    }

    public static CanFail<Route> Create(List<EvaNumber> stops, List<AllowedVehicles> allowedVehicles)
    {
        if (stops.Count() < 2) return DomainErrors.ConnectionRequest.Route.Min2Stops;
        if (stops.Count() > 4) return DomainErrors.ConnectionRequest.Route.Max2Stopovers;
        if (allowedVehicles.Count() != stops.Count() - 1)
            return DomainErrors.ConnectionRequest.Route.AllowedVehiclesMismatch;
        if (allowedVehicles.Any(allowed => allowed is
            {
                HighSpeed: false,
                Intercity: false,
                Regional: false,
                PublicTransport: false
            }))
        {
            return DomainErrors.ConnectionRequest.Route.NoVehicleAllowed;   
        }
        
        return new Route(stops, allowedVehicles);
    }
}