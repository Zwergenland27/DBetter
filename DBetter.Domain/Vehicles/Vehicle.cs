using System.Text.RegularExpressions;
using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

//TODO: Rename to vehicleType and make vehicle the concrete one
public class Vehicle : AggregateRoot<VehicleId>
{
    private List<Coach> _coachSequence;
    
    public string Identifier { get; private set; }

    public int MaxAllowedCouplings { get; private set; } = 3;
    
    public IReadOnlyList<Coach> CoachSequence => _coachSequence.AsReadOnly();
    
    private Vehicle() : base(null!)
    {
    }

    private Vehicle(VehicleId id, string identifier, List<Coach> coachSequence) : base(id)
    {
        Identifier = identifier;
        _coachSequence = coachSequence;
    }

    public static Vehicle CreateByConstructionType(string identifier, List<string> coachSequence)
    {
        var coaches = new List<Coach>();
        if ((coachSequence.First() is "I4015" && coachSequence.Last() is "I4010") ||
            coachSequence.First() is "I4010" && coachSequence.Last() is "I4015")
        {
            identifier = "401";
        }

        byte coachId = 0;
        foreach (var coachConstructionType in coachSequence)
        {
            coaches.Add(Coach.CreateByConstructionType(new CoachId(coachId), coachConstructionType));
            coachId++;
        }
        return new Vehicle(VehicleId.CreateNew(), identifier, coaches);
    }
    
    public static Vehicle CreateByCoachType(List<string> coachSequence)
    {
        coachSequence = coachSequence.Select(RemoveCoachNumber).ToList();
        var coaches = new List<Coach>();

        byte coachId = 0;
        foreach (var coachType in coachSequence)
        {
            coaches.Add(Coach.CreateByCoachType(new CoachId(coachId), coachType));
            coachId++;
        }
        return new Vehicle(VehicleId.CreateNew(), string.Join("|", coachSequence), coaches);
    }

    public bool MatchesConstructionType(List<string> coachSequence)
    {
        var normalOrder = string.Join('|', coachSequence);
        coachSequence.Reverse();
        var reversedOrder = string.Join('|', coachSequence);

        var coachConstructionType = string.Join('|', _coachSequence.Select(cs => cs.ConstructionType));
        return normalOrder == coachConstructionType ||  reversedOrder == coachConstructionType;
    }
    
    public bool MatchesCoachType(List<string> coachSequence)
    {
        coachSequence = coachSequence.Select(RemoveCoachNumber).ToList();
        var normalOrder = string.Join('|', coachSequence);
        coachSequence.Reverse();
        var reversedOrder = string.Join('|', coachSequence);

        var coachConstructionType = string.Join('|', _coachSequence.Where(cs => cs.CoachType is not null).Select(cs => cs.CoachType));
        return normalOrder == coachConstructionType ||  reversedOrder == coachConstructionType;
    }

    private static string RemoveCoachNumber(string coach)
    {
        return Regex.Replace(coach, @"-\d+$", "");
    }
}