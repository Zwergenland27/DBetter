using System.Text.RegularExpressions;
using CleanDomainValidation.Domain;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

public class Br401Factory
{
    internal static Error InvalidCoachFormat => Error.Validation("Internal.Br401Factory.InvalidCoachFormat", "The coach does not have the expected format.");
    private static Coach FirstLocomotive => Coach.CreateByConstructionType(new CoachId(0), "I4010");
    private static Coach RestaurantCoach => Coach.CreateByConstructionType(new CoachId(7), "I8040");
    private static Coach LastLocomotive => Coach.CreateByConstructionType(new CoachId(10), "I4015");
    
    internal CanFail<List<Coach>> GenerateFromCoachTypes(List<string> coachTypes)
    {
        var coachNumbers = new List<short>();
        foreach (var coachType in coachTypes)
        {
            var coachNumberResult = ExtractCoachNumber(coachType);
            if (coachNumberResult.HasFailed) return coachNumberResult.Errors;
            
            coachNumbers.Add(coachNumberResult.Value);
        }
        
        if (coachNumbers.First() < 8020)
        {
            coachTypes.Reverse();
            coachNumbers.Reverse();
        }
        
        var coaches = new List<Coach>()
        {
            FirstLocomotive,
            RestaurantCoach,
            LastLocomotive
        };

        byte coachId = 1;
        for (var i = 0; i < coachNumbers.Count; i++)
        {
            var coachNumber = coachNumbers[i];
            var coachType = coachTypes[i];
            
            coaches.Add(Coach.Create(new CoachId(coachId), $"I{coachNumber}", coachType));
            coachId++;
            
            if (coachId is 7)
            {
                coachId++;
            }
        }

        return coaches;
    }

    internal CanFail<List<Coach>> GenerateFromConstructionTypes(List<string> constructionTypes)
    {
        constructionTypes.Remove("I4010");
        constructionTypes.Remove("I4015");
        constructionTypes.Remove("I8040");
        var coachNumbers = new List<short>();
        foreach (var constructionType in constructionTypes)
        {
            var coachNumberResult = ExtractConstructionNumber(constructionType);
            if (coachNumberResult.HasFailed) return coachNumberResult.Errors;
            
            coachNumbers.Add(coachNumberResult.Value);
        }
        
        if (coachNumbers.First() < 8020)
        {
            coachNumbers.Reverse();
        }
        
        var coaches = new List<Coach>()
        {
            FirstLocomotive,
            RestaurantCoach,
            LastLocomotive
        };

        byte coachId = 1;
        foreach(var coachNumber in coachNumbers)
        {
            var coachType = $"I{coachNumber}-401.LDV";
            
            coaches.Add(Coach.Create(new CoachId(coachId), $"I{coachNumber}", coachType));
            coachId++;
            
            if (coachId is 7)
            {
                coachId++;
            }
        }

        return coaches;
    }

    private static CanFail<short> ExtractCoachNumber(string coachType)
    {
        var match = Regex.Match(coachType, @"I(\d+)-");
        return match.Success ? short.Parse(match.Groups[1].Value): InvalidCoachFormat;
    }

    private static CanFail<short> ExtractConstructionNumber(string constructionType)
    {
        var match = Regex.Match(constructionType, @"I(\d+)");
        return match.Success ? short.Parse(match.Groups[1].Value): InvalidCoachFormat;
    }
}