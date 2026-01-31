using DBetter.Application.TrainRuns.Dtos;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainRuns.Snapshots;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainRuns;

public interface IExternalTrainRunProvider
{
    Task<TrainRunDto> GetTrainRunAsync(BahnJourneyId journeyId);
}