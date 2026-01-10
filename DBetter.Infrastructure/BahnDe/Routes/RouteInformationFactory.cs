using DBetter.Domain.Routes.ValueObjects;
using DBetter.Infrastructure.BahnDe.Routes.DTOs;
using DBetter.Infrastructure.BahnDe.Shared;

namespace DBetter.Infrastructure.BahnDe.Routes;

public class RouteInformationFactory(Fahrt fahrt)
{
    public List<ServiceNumber> ExtractServiceNumbers()
    {
        var serviceNumbers = new List<ServiceNumber>();
        foreach (var halt in fahrt.Halte)
        {
            if (halt.Nummer is null) continue;
            var serviceNumberResult = ServiceNumber.Create(halt.Nummer);
            if (serviceNumberResult.HasFailed) continue;
            serviceNumbers.Add(serviceNumberResult.Value);
        }

        return serviceNumbers.Distinct().ToList();
    }
    
    public CateringInformation ExtractCateringInformation()
    {
        return new CateringInformationFactory(fahrt).ExtractInformation();
    }
    
    public BikeCarriageInformation ExtractBikeCarriageInformation()
    {
        return new BikeCarriageInformationFactory(fahrt).ExtractInformation();
    }

    public List<PassengerInformation> ExtractPassengerInformationMessages()
    {
        return [];
    }
}