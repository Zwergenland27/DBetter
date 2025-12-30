using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.Shared;

public class PassengerInformationFactory(IHasMessage routeWithMessages)
{
    
    public List<PassengerInformation> ExtractInformation()
    {
        var messages = new List<PassengerInformation>();
        if (routeWithMessages.HimMeldungen is not null)
        {
            foreach (var himMeldung in routeWithMessages.HimMeldungen)
            {
                if(himMeldung.Text is null) continue;
                messages.Add(PassengerInformation.CreateUnknown(himMeldung.Text));
            }   
        }

        foreach (var risNotiz in routeWithMessages.RisNotizen)
        {
            messages.Add(PassengerInformation.CreateUnknown(risNotiz.Value));
        }

        foreach (var prioritized in routeWithMessages.PriorisierteMeldungen)
        {
            messages.Add(PassengerInformation.CreateUnknown(prioritized.Text));
        }

        return messages.Distinct().ToList();
    }
}