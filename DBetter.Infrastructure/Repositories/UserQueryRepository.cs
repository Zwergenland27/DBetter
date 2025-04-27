using DBetter.Application;
using DBetter.Application.Users;
using DBetter.Contracts.Users.Queries.GetMyPassengers;
using DBetter.Domain.Users.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class UserQueryRepository(DBetterContext context) : IUserQueryRepository
{
    public async Task<MyPassengersResult?> GetUserPassengers(UserId userId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null) return null;
        
        return new MyPassengersResult
        {
            Me = new PassengerResult
            {
                Id = user.Id.Value.ToString(),
                Firstname = user.Firstname.Value,
                Lastname = user.Lastname.Value,
                Email = user.Email.Value,
                Birthday = user.Birthday.Utc.ToIso8601(),
                Discounts = user.Discounts.Select(discount => new DiscountResult
                {
                    Type = discount.Type.ToString(),
                    Class = discount.ComfortClass.ToString(),
                    ValidUntil = discount.ValidUntilUtc?.ToIso8601()
                }).ToList()
            },
            Friends = [],
            Family = []
        };
    }
}