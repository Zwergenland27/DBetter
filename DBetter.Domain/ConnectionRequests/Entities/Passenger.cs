using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.Entities;

public class Passenger : Entity<PassengerId>
{
    private List<PassengerDiscount> _discounts;
    
    public UserId? UserId { get; private set; }
    
    public PassengerName? Name { get; private set; }
    
    public Birthday? Birthday { get; private set; }
    
    public int? Age { get; private set; }

    public bool OwnsDeutschlandTicket => Discounts.Any(d => d.Type is DiscountType.DeutschlandTicket);
    
    public int Bikes { get; private set; }
    
    public int Dogs { get; private set; }
    
    public IReadOnlyList<PassengerDiscount> Discounts => _discounts.AsReadOnly();

    private Passenger() : base(null!){}
    
    private Passenger(
        PassengerId id,
        UserId? userId,
        PassengerName? name,
        Birthday? birthday,
        int? age,
        int bikes,
        int dogs,
        List<PassengerDiscount> discounts) : base(id)
    {
        UserId = userId;
        Name = name;
        Birthday = birthday;
        Age = age;
        Bikes = bikes;
        Dogs = dogs;
        _discounts = discounts;
    }

    public static CanFail<Passenger> Create(
        PassengerId id,
        UserId? userId,
        PassengerName? name,
        Birthday? birthday,
        int? age,
        int bikes,
        int dogs,
        List<PassengerDiscount> discounts)
    {
        if (birthday is null && age is null) return DomainErrors.ConnectionRequest.Passenger.MissingAgeField(id);

        var discountsAdditionalToDeutschlandTicket = discounts.Count(d => d.Type is not DiscountType.DeutschlandTicket);
        
        if(discountsAdditionalToDeutschlandTicket > 4) return DomainErrors.ConnectionRequest.Passenger.Max4Discounts(id);

        if (discounts.Distinct().Count() != discounts.Count)
            return DomainErrors.ConnectionRequest.Passenger.DuplicateDiscounts(id);
        
        return new Passenger(id, userId, name, birthday, age, bikes, dogs, discounts);
    }
}