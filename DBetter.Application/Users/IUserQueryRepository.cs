using DBetter.Contracts.Users.Queries.GetMyPassengers;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users;

public interface IUserQueryRepository
{
    Task<MyPassengersResult?> GetUserPassengers(UserId userId);
}