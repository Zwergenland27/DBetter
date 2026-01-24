using DBetter.Application.Shared;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.BahnDe.Shared;
using DBetter.Infrastructure.BahnDe.TrainRuns.DTOs;

namespace DBetter.Infrastructure.BahnDe.TrainRuns;

public class TrainRunInformationFactory(Fahrt fahrt)
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

    public List<PassengerInformationDto> ExtractPassengerInformation()
    {
        return new PassengerInformationTextFactory(fahrt).ExtractInformation();
    }
}