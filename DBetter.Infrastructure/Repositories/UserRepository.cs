using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class UserRepository(DBetterContext context) : IUserRepository
{
    public Task<User?> GetAsync(UserId id)
    {
        return context.Users
            .FirstOrDefaultAsync(user => user.Id == id);
    }

    public Task<User?> GetByEmailAsync(Email email)
    {
        return context.Users
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await context.Users.AddAsync(user);
    }

    public Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        return Task.CompletedTask;
    }
}