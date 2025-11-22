using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetAsync(UserId id);
    Task<User?> GetByEmailAsync(Email email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
}