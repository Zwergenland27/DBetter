using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Domain.TrainCompositions;
using DBetter.Domain.TrainCompositions.ValueObjects;
using DBetter.Domain.TrainRuns.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class SimpleTrainCompositionPredictor(DBetterContext db) : ITrainCompositionPredictor
{
    private const double SourceWeight_RealTime = 3.0;
    private const double SourceWeight_SeatingPlan = 1.0;
    private const double SourceWeight_Historical = 0.3;

    private const double DayMatch_SameDay = 2.0;
    private const double DayMatch_Weekend = 1.2;
    private const double DayMatch_None = 0.7;

    private const double RecencyDecayPerDay = 0.01;
    
    private record TrainCompositionOfOperatingDay(TrainComposition TrainComposition, OperatingDay OperatingDay);
    
    public async Task<PredictionResult?> PredictAsync(TrainCirculationId trainCirculationId, OperatingDay operatingDay)
    {
        var pastTrainCompositions = await db.TrainCompositions
            .Join(db.TrainRuns,
                tc => tc.TrainRun,
                tr => tr.Id,
                (tc, tr) => new { TrainComposition = tc, tr.OperatingDay, TrainCirculationId = tr.CirculationId })
            .Where(r => r.TrainCirculationId == trainCirculationId)
            .Select(r => new TrainCompositionOfOperatingDay(r.TrainComposition, r.OperatingDay))
            .ToListAsync();

        if (pastTrainCompositions.Count == 0)
        {
            return null;
        }
        
        var scores = new Dictionary<string, double>();

        foreach (var pastTrainComposition in pastTrainCompositions)
        {
            var identifier = pastTrainComposition.TrainComposition.CalculateIdentifier();
            var score = CalculateScore(pastTrainComposition, operatingDay);
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

    private static double CalculateScore(TrainCompositionOfOperatingDay pastTrainComposition, OperatingDay operatingDay)
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

    private static double GetDayWeight(OperatingDay sourceOperatingDay, OperatingDay targetOperatingDay)
    {
        DayOfWeek entryDay  = sourceOperatingDay.Date.DayOfWeek;
        DayOfWeek targetDay = targetOperatingDay.Date.DayOfWeek;

        if (entryDay == targetDay)
            return DayMatch_SameDay;

        bool entryIsWeekend  = entryDay is DayOfWeek.Saturday or DayOfWeek.Sunday;
        bool targetIsWeekend = targetDay is DayOfWeek.Saturday or DayOfWeek.Sunday;

        return entryIsWeekend == targetIsWeekend
            ? DayMatch_Weekend
            : DayMatch_None;
    }

    private static double GetRecencyFactor(OperatingDay sourceOperatingDay, OperatingDay targetOperatingDay)
    {
        int daysDiff = Math.Abs(targetOperatingDay.Date.DayNumber - sourceOperatingDay.Date.DayNumber);
        return Math.Exp(-RecencyDecayPerDay * daysDiff);
    }
}