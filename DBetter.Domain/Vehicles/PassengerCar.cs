using DBetter.Domain.CoachLayouts.ValueObjects;
using DBetter.Domain.Vehicles.ValueObjects;

namespace DBetter.Domain.Vehicles;

public class PassengerCar: Vehicle
{
    public bool IsControlCar { get; private set; }
    
    public bool IsDoubleDecker { get; private set; }
    
    public CoachLayoutId? CoachLayout { get; private set; }
    
    public PassengerCar(
        VehicleId id,
        EuropeanVehicleNumber evn,
        bool isControlCar,
        bool isDoubleDecker,
        CoachLayoutId? coachLayout) : base(id, evn)
    {
        IsControlCar = isControlCar;
        IsDoubleDecker = isDoubleDecker;
        CoachLayout = coachLayout;
    }
}