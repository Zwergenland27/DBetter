using CleanDomainValidation.Domain;
using DBetter.Domain.Abstractions;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Errors;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.Entities;

public class Passenger : Entity<PassengerId>
{
    private List<PassengerDiscount> _discounts;
    
    public UserId? UserId { get; set; }
    
    public PassengerName? Name { get; set; }
    
    public Birthday? Birthday { get; set; }
    
    public int? Age { get; set; }
    
    public PassengerOptions Options { get; set; }
    
    public IReadOnlyList<PassengerDiscount> Discounts => _discounts.AsReadOnly();

    private Passenger() : base(null!){}
    
    private Passenger(
        PassengerId id,
        UserId? userId,
        PassengerName? name,
        Birthday? birthday,
        int? age,
        PassengerOptions options,
        List<PassengerDiscount> discounts) : base(id)
    {
        UserId = userId;
        Name = name;
        Birthday = birthday;
        Age = age;
        Options = options;
        _discounts = discounts;
    }

    public static CanFail<Passenger> Create(
        PassengerId id,
        UserId? userId,
        PassengerName? name,
        Birthday? birthday,
        int? age,
        PassengerOptions options,
        List<PassengerDiscount> discounts)
    {
        if (birthday is null && age is null) return DomainErrors.ConnectionRequest.Passenger.MissingAgeField(id);
        if(discounts.Count() > 4) return DomainErrors.ConnectionRequest.Passenger.Max4Discounts(id);

        if (discounts.Distinct().Count() != discounts.Count())
            return DomainErrors.ConnectionRequest.Passenger.DuplicateDiscounts(id);
        
        return new Passenger(id, userId, name, birthday, age, options, discounts);
    }
}