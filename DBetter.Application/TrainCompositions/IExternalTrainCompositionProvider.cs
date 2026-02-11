using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Application.TrainCompositions;

public interface IExternalTrainCompositionProvider
{
    Task<TrainCompositionDto?> GetRealTimeDataAsync(ServiceNumber ServiceNumber, DateOnly Date, EvaNumber AtStation);
}