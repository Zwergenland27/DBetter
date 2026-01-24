using DBetter.Application.TrainRuns.Dtos;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainRuns;

public interface IExternalTrainRunProvider
{
    Task<TrainRunDto> GetTrainRunAsync(BahnJourneyId journeyId);
}