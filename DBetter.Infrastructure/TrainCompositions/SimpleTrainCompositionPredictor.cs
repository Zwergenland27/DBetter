using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.TrainCompositions;

public class SimpleTrainCompositionPredictor(DBetterContext db) : ITrainCompositionPredictor
{
    private const double SourceWeight_RealTime = 3.0;
    private const double SourceWeight_SeatingPlan = 1.0;
    private const double SourceWeight_Historical = 0.3;

    private const double DayMatch_SameDay = 2.0;
    private const double DayMatch_Weekend = 1.2;
    private const double DayMatch_None = 0.7;

    private const double RecencyDecayPerDay = 0.01;
    
    private record TrainCompositionOfOperatingDay(TrainComposition TrainComposition, DateOnly OperatingDay);
    
    public async Task<PredictionResult?> PredictAsync(TrainCirculationId trainCirculationId, OperatingDay operatingDay)
    {
        var pastTrainCompositionsDtos = await db.TrainCompositions
            .Join(db.TrainRuns,
                tc => tc.TrainRunId,
                tr => tr.Id,
                (tc, tr) => new { TrainComposition = tc, tr.OperatingDay, tr.TrainCirculationId })
            .Where(r => r.TrainCirculationId == trainCirculationId.Value && r.TrainComposition.Source != (int) TrainFormationSource.None)
            .Select(r => new {r.TrainComposition, r.OperatingDay})
            .ToListAsync();
        
        var pastTrainCompositions = pastTrainCompositionsDtos
            .Select(ptc => new TrainCompositionOfOperatingDay(ptc.TrainComposition.ToDomain(), ptc.OperatingDay))
            .ToList();

        if (pastTrainCompositions.Count == 0)
        {
            return null;
        }
        
        var scores = new Dictionary<string, double>();

        foreach (var pastTrainComposition in pastTrainCompositions)
        {
            var identifier = pastTrainComposition.TrainComposition.CalculateIdentifier();
            var score = CalculateScore(pastTrainComposition, operatingDay.Date);
            if (!scores.TryAdd(identifier, score))
            {
                scores[identifier] = score;
            }
        }
        
        var maxScore = scores.Values.Max();
        var expScores = scores.ToDictionary(kvp => kvp.Key, kvp => Math.Exp(kvp.Value - maxScore));
        
        var sumExp = expScores.Values.Sum();
        var predictionResult = expScores
            .Select(kvp => new { kvp.Key, Score = scores[kvp.Key], Propability = kvp.Value / sumExp })
            .OrderByDescending(r => r.Propability)
            .First();

        return new PredictionResult(
            pastTrainCompositions.First(ptc =>
                    ptc.TrainComposition.CalculateIdentifier() == predictionResult.Key)
                .TrainComposition,
            predictionResult.Score,
            predictionResult.Propability
        );
    }

    private static double CalculateScore(TrainCompositionOfOperatingDay pastTrainComposition, DateOnly operatingDay)
    {
        var sourceWeight = GetSourceWeight(pastTrainComposition.TrainComposition.Source);
        var dayWeight = GetDayWeight(pastTrainComposition.OperatingDay, operatingDay);
        var recencyFactor = GetRecencyFactor(pastTrainComposition.OperatingDay, operatingDay);

        return sourceWeight * dayWeight * recencyFactor;
    }
    
    private static double GetSourceWeight(TrainFormationSource source) => source switch
    {
        TrainFormationSource.RealTime    => SourceWeight_RealTime,
        TrainFormationSource.SeatingPlan => SourceWeight_SeatingPlan,
        TrainFormationSource.Prediction  => SourceWeight_Historical,
        _                  => SourceWeight_SeatingPlan
    };

    private static double GetDayWeight(DateOnly sourceOperatingDay, DateOnly targetOperatingDay)
    {
        DayOfWeek entryDay  = sourceOperatingDay.DayOfWeek;
        DayOfWeek targetDay = targetOperatingDay.DayOfWeek;

        if (entryDay == targetDay)
            return DayMatch_SameDay;

        bool entryIsWeekend  = entryDay is DayOfWeek.Saturday or DayOfWeek.Sunday;
        bool targetIsWeekend = targetDay is DayOfWeek.Saturday or DayOfWeek.Sunday;

        return entryIsWeekend == targetIsWeekend
            ? DayMatch_Weekend
            : DayMatch_None;
    }

    private static double GetRecencyFactor(DateOnly sourceOperatingDay, DateOnly targetOperatingDay)
    {
        int daysDiff = Math.Abs(targetOperatingDay.DayNumber - sourceOperatingDay.DayNumber);
        return Math.Exp(-RecencyDecayPerDay * daysDiff);
    }
}