using DBetter.Application;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Infrastructure.BahnDe.TrainCompositions.Planned;

public class RequestBuilder(ServiceNumber serviceNumber, EvaNumber originStation, DateTime departureTime, EvaNumber destinationStation, DateTime arrivalTime)
{
    public RootParameters Build()
    {
        return new RootParameters
        {
            Buchungskontext = new()
            {
                BuchungsKontextDaten = new BuchungsKontextDaten
                {
                    Zugnummer = serviceNumber.Value.ToString(),
                    AbfahrtHalt = new StartHalt
                    {
                        LocationId = originStation.Value,
                        AbfahrtZeit = departureTime.ToBahnTime()
                    },
                    AnkunftHalt = new EndHalt
                    {
                        LocationId = destinationStation.Value,
                        AnkunftZeit = arrivalTime.ToBahnTime()
                    }
                }
            }
        };
    }
}