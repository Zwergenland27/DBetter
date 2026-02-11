using CleanMediator.Queries;
using DBetter.Contracts.TrainCompositions.Get;
using DBetter.Domain.TrainRuns.ValueObjects;

namespace DBetter.Application.TrainCompositions.Get;

public record GetTrainCompositionQuery(TrainRunId TrainRunId) : IQuery<GetTrainCompositionResultDto>;