using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionRequestRepository(DBetterContext context) : IConnectionRequestRepository
{
    public Task<ConnectionRequest?> GetAsync(ConnectionRequestId id)
    {
        return context.ConnectionRequests
            .FirstOrDefaultAsync(cr => cr.Id == id);
    }

    public async Task AddAsync(ConnectionRequest connectionRequest)
    {
        await context.ConnectionRequests.AddAsync(connectionRequest);
    }
}