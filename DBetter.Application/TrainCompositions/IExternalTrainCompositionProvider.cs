using DBetter.Application.TrainCompositions.Dtos;
using DBetter.Domain.Routes.Stops;
using DBetter.Domain.Stations.ValueObjects;
using DBetter.Domain.TrainCirculations.ValueObjects;

namespace DBetter.Application.TrainCompositions;

public interface IExternalTrainCompositionProvider
{
    Task<TrainCompositionDto?> GetRealTimeDataAsync(ServiceNumber ServiceNumber, DateOnly Date, EvaNumber AtStation);

    Task<PlannedTrainCompositionDto?> GetPlannedDataAsync(ServiceNumber serviceNumber, EvaNumber originStation, DateTime deparureTime, EvaNumber destinationStation, DateTime arrivalTime);
}